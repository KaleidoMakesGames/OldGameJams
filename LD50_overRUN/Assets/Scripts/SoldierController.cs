using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoldierController : MonoBehaviour, IBoatLoadable
{
    public static List<SoldierController> soldiers;

    [Header("Movement")]
    public KMGPathfinding.AgentController agent;

    [Header("Attacking")]
    public bool isCivilian = false;
    public float attackRange;
    public bool attackOverWalls {
        get {
            return currentBuilding != null;
        }
    }
    public float attackCooldown;
    public float attackDamage;
    public ProjectileController projectile;
    public Vector3 shootPoint;
    [ReadOnly] [SerializeField] private float cooldownRemaining;
    private float lastAttackTime;
    private ZombieController targetZombie;

    public Transform deathParticle;

    [Header("Health")]
    public float maxHealth; 
    [SerializeField] private float _currentHealth;

    public Sprite boatSprite;
    public Color boatSpriteColor;

    public float buildingTransitionTime;

    [ReadOnly]public bool animating;

    [ReadOnly] public bool tryToBoard;
    [ReadOnly] public BuildingController buildingToTryToEnter;
    [ReadOnly] public BuildingController currentBuilding;

    public float currentHealth {
        get {
            return _currentHealth;
        }
        set {
            _currentHealth = Mathf.Clamp(value, 0, maxHealth);
        }
    }

    private void Awake() {
        if(soldiers == null) {
            soldiers = new List<SoldierController>();
        }
        currentHealth = maxHealth;
        lastAttackTime = Mathf.NegativeInfinity;
    }

    private void OnEnable() {
        soldiers.Add(this);
    }

    private void OnDisable() {
        soldiers.Remove(this);
    }

    public Sprite GetSprite() {
        return boatSprite;
    }

    public Color GetColor() {
        return boatSpriteColor;
    }

    private void Update() {
        if(animating) {
            return;
        }
        if(currentHealth <= 0) {
            GameManager.instance.deathsSoFar++;
            GameManager.instance.audioSource.PlayOneShot(GameManager.instance.playerDead);
            Instantiate(deathParticle.gameObject).transform.position = transform.position;
            Destroy(gameObject);
            return;
        }


        if (isCivilian) {
            if(Island.AtPoint(transform.position).location == Boat.Location.Freedom) {
                if(agent.enabled) {
                    GameManager.instance.audioSource.PlayOneShot(GameManager.instance.alright);
                }
                agent.enabled = false;
                transform.Translate(Vector2.right * Time.deltaTime * agent.movementSpeed * 2);
            }
            if(transform.position.x >= 9) {
                Destroy(gameObject);
                GameManager.instance.rescued++;
            }
        } else {
            UpdateTarget();

            cooldownRemaining = Mathf.Max(0, attackCooldown - (Time.time - lastAttackTime));
            if (targetZombie != null && cooldownRemaining == 0) {
                DoAttack();
            }
        }

        if(currentBuilding != null) {
            transform.position = currentBuilding.roofSpot.position;
        }

        if(tryToBoard && !agent.isMoving) {
            // Check if we are at a boarding location
            var dockZone = Physics2D.OverlapPointAll(transform.position).FirstOrDefault(x => x.GetComponentInParent<DockZone>() != null);
            if(dockZone != null) {
                var currentLocation = dockZone.GetComponent<DockZone>().location;
                if(Boat.instance.isDocked && Boat.instance.currentLocation == currentLocation) {
                    if(!Boat.instance.TryLoad(this)) {
                        tryToBoard = false;
                    }
                }
            }
        }

        if(currentBuilding == null && buildingToTryToEnter != null && !agent.isMoving) {
            var buildingToEnter = Physics2D.OverlapPointAll(transform.position).FirstOrDefault(x => x.GetComponentInParent<BuildingController>() == buildingToTryToEnter);
            if(buildingToEnter && buildingToTryToEnter.unitOnRoof == null) {
                StartCoroutine(EnterBuilding(buildingToTryToEnter));
            }
        }
    }

    public void Load() {
        tryToBoard = false;
        agent.Stop();
        gameObject.SetActive(false);
    }

    public void Unload(Vector2 position) {
        gameObject.SetActive(true);
        transform.position = position;
    }


    public void MoveTo(Vector2 position) {
        agent.destination = position;
        if (currentBuilding != null) {
            agent.enabled = false;
            StartCoroutine(LeaveBuilding());
        }
    }

    private IEnumerator EnterBuilding(BuildingController buildingToEnter) {
        buildingToTryToEnter = null;
        currentBuilding = buildingToEnter;
        buildingToEnter.unitOnRoof = this;
        animating = true;
        Vector2 startPos = transform.position;
        Vector2 endPos = buildingToEnter.groundDoor.position;
        Vector2 startScale = transform.localScale;
        float t = 0;
        while(t < 1) {
            t += Time.deltaTime / buildingTransitionTime;
            transform.position = Vector2.Lerp(startPos, endPos, t);
            transform.localScale = Vector2.Lerp(startScale, Vector2.zero, t);
            yield return null;
        }
        t = 0;
        transform.position = buildingToEnter.roofDoor.position;
        while (t < 1) {
            t += Time.deltaTime / buildingTransitionTime;
            transform.localScale = Vector2.Lerp(Vector2.zero, startScale, t);
            yield return null;
        }
        t = 0;
        while (t < 1) {
            t += Time.deltaTime / buildingTransitionTime;
            transform.position = Vector2.Lerp(buildingToEnter.roofDoor.position, buildingToEnter.roofSpot.position, t);
            yield return null;
        }
        animating = false;
        transform.position = buildingToEnter.roofSpot.position;
    }

    private IEnumerator LeaveBuilding() {
        currentBuilding.unitOnRoof = null;
        animating = true;
        var buildingToExit = currentBuilding;
        currentBuilding = null;
        Vector2 startScale = transform.localScale;
        float t = 0;
        while (t < 1) {
            t += Time.deltaTime / buildingTransitionTime;
            transform.position = Vector2.Lerp(buildingToExit.roofSpot.position, buildingToExit.roofDoor.position, t);
            yield return null;
        }
        t = 0;
        while (t < 1) {
            t += Time.deltaTime / buildingTransitionTime;
            transform.localScale = Vector2.Lerp(startScale, Vector2.zero, t);
            yield return null;
        }
        t = 0;
        while (t < 1) {
            t += Time.deltaTime / buildingTransitionTime;
            transform.position = Vector2.Lerp(buildingToExit.groundDoor.position, buildingToExit.groundSpot.position, t);
            transform.localScale = Vector2.Lerp(Vector2.zero, startScale, t);
            yield return null;
        }
        transform.position = buildingToExit.groundSpot.position;
        animating = false;
        agent.enabled = true;
        agent.destination = agent.destination;
    }

    private void DoAttack() {
        lastAttackTime = Time.time;

        GameManager.instance.audioSource.PlayOneShot(GameManager.instance.shoot);
        var spawnedProjectile = Instantiate(projectile).GetComponent<ProjectileController>();
        spawnedProjectile.spawnPosition = transform.TransformPoint(shootPoint);
        spawnedProjectile.homingTarget = targetZombie.transform;
        spawnedProjectile.OnTargetLost.AddListener(delegate {
            Destroy(spawnedProjectile.gameObject);
        });
        spawnedProjectile.OnReachedTarget.AddListener(delegate {
            if (spawnedProjectile.homingTarget != null) {
                spawnedProjectile.homingTarget.GetComponentInParent<ZombieController>().currentHealth -= attackDamage;
            }
        });
    }

    private void UpdateTarget() {
        var zombiesInRange = FindObjectsOfType<ZombieController>().OrderBy(x => Vector2.Distance(x.transform.position, transform.position) <= attackRange);

        targetZombie = null;
        foreach(var zombie in zombiesInRange) {
            if (!attackOverWalls && !HasLOS(zombie.transform.position)) {
                continue;
            }
            float distance = Vector2.Distance(transform.position, zombie.transform.position);
            if(distance <= attackRange) {
                targetZombie = zombie;
                return;
            }
        }
    }

    bool HasLOS(Vector2 position) {
        Physics2D.queriesHitTriggers = false;
        var v = position - (Vector2)transform.position;
        var hit = Physics2D.Raycast(transform.position, v, v.magnitude, ~LayerMask.GetMask("IgnoreEachother"));
        Physics2D.queriesHitTriggers = true;
        return hit.collider == null;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.DrawWireSphere(transform.TransformPoint(shootPoint), 0.1f);
        if (targetZombie != null) {
            Gizmos.DrawLine(transform.position, targetZombie.transform.position);
        }
    }
}
