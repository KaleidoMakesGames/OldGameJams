using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ZombieController : MonoBehaviour
{
    public static List<ZombieController> zombiesList;

    [Header("Movement")]
    public float roamingSpeed;
    [Range(0, 1)] public float agility;
    public float frenziedSpeed;
    [ReadOnly] public bool isFrenzied;

    public Transform onDeath;

    [Header("Movement Logic")]
    public float avoidZombiesStrength;
    public float avoidZombiesRange;
    public int avoidZombiesFalloff;
    public float avoidWallsStrength;
    public float avoidWallsRange;
    public int avoidWallsFalloff;
    public int wallSensorCount;
    public float huntSoldiersStrength;
    public float huntSoldiersRange;
    public int huntSoldiersFalloff;
    public bool isIdle;
    public float soldierTetherDistance;
    public float frenziedDistance;

    [Header("Attacks")]
    public float attackDistance;
    public float attackCooldown;
    public float attackDamage;
    [ReadOnly] [SerializeField] private float cooldownRemaining;

    [Header("Health")]
    public float maxHealth;
    [ReadOnly] [SerializeField] private float _currentHealth;
    public float currentHealth {
        get {
            return _currentHealth;
        }
        set {
            _currentHealth = Mathf.Clamp(value, 0, maxHealth);
        }
    }

    private bool[] blockedWalls;
    private float[] wallDistances;
    private Vector2[] wallDirections;
    private float lastAttackTime;
    private SoldierController targetedSoldier;

    private Rigidbody2D rb;

    private void Awake() {
        if(zombiesList == null) {
            zombiesList = new List<ZombieController>();
        }
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        lastAttackTime = Mathf.NegativeInfinity;
    }

    private void OnEnable() {
        zombiesList.Add(this);
    }

    private void OnDisable() {
        zombiesList.Remove(this);
    }

    private void Update() {
        if(currentHealth == 0) {
            GameManager.instance.zombiesSlain++;
            GameManager.instance.audioSource.PlayOneShot(GameManager.instance.zombieDead);
            Instantiate(onDeath.gameObject).transform.position = transform.position;
            Destroy(gameObject);
        }
        cooldownRemaining = Mathf.Max(0, attackCooldown - (Time.time - lastAttackTime));
        if(targetedSoldier != null && Vector2.Distance(transform.position, targetedSoldier.transform.position) <= attackDistance && cooldownRemaining == 0) {
            DoAttack();
        }
    }

    private void DoAttack() {
        targetedSoldier.currentHealth -= attackDamage;
        lastAttackTime = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (wallDirections == null || wallDirections.Length != wallSensorCount) {
            wallDirections = new Vector2[wallSensorCount];
            blockedWalls = new bool[wallSensorCount];
            wallDistances = new float[wallSensorCount];
        }
        Physics2D.queriesHitTriggers = false;
        for (int i = 0; i < wallSensorCount; i++) {
            float angle = Mathf.Deg2Rad * i * 360.0f / wallSensorCount;
            Vector2 v = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            var hit = Physics2D.Raycast(transform.position, v, avoidWallsRange, ~LayerMask.GetMask("IgnoreEachother"));
            blockedWalls[i] = hit.collider != null;
            wallDirections[i] = v;
            wallDistances[i] = hit.distance;
        }
        rb.velocity = Vector2.Lerp(rb.velocity, GetDesiredMovementVector().normalized * (isFrenzied ? frenziedSpeed : roamingSpeed), agility);
        Physics2D.queriesHitTriggers = true;
    }

    Vector2 GetDesiredMovementVector() {
        return AvoidZombiesVector() + AvoidWallsVector() + HuntSoldiersVector();
    }

    Vector2 AvoidWallsVector() {
        Physics2D.queriesHitTriggers = false;

        Vector2 avoidWall = Vector2.zero;
        for(int i = 0; i < wallSensorCount; i++) {
            if (blockedWalls[i]) {
                avoidWall += -wallDirections[i] * Mathf.Pow(avoidWallsRange - wallDistances[i], avoidWallsFalloff);
            }
        }

        Physics2D.queriesHitTriggers = true;

        return avoidWallsStrength * avoidWall / (wallSensorCount * Mathf.Pow(avoidWallsRange, avoidWallsFalloff));
    }

    Vector2 AvoidZombiesVector() {
        var zombiesInRange = zombiesList.Where(x => x != this && Vector2.Distance(x.transform.position, transform.position) < avoidZombiesRange);
        Vector2 avoidZombies = Vector2.zero;
        foreach(var zombie in zombiesInRange) {
            Vector2 delta = transform.position - zombie.transform.position;
            avoidZombies += delta.normalized * Mathf.Pow((avoidZombiesRange - delta.magnitude), avoidZombiesFalloff);
        }
        return avoidZombiesStrength * avoidZombies / Mathf.Pow(avoidZombiesRange, avoidZombiesFalloff);
    }

    Vector2 HuntSoldiersVector() {
        var soldiersInRange = SoldierController.soldiers.Where(x => Vector2.Distance(x.transform.position, transform.position) <= huntSoldiersRange && x.currentBuilding == null);
        targetedSoldier = null;
        SoldierController closestInLineOfSight = null;
        SoldierController closestOverall = null;
        float closestDistance = Mathf.Infinity;
        float closestLOSDistance = Mathf.Infinity;
        foreach (var soldier in soldiersInRange) {
            float distance = Vector2.Distance(soldier.transform.position, transform.position);
            if (distance < closestDistance) {
                closestOverall = soldier;
                closestDistance = distance;
            }
            if(HasLOS(soldier.transform.position) && distance < closestLOSDistance) {
                closestInLineOfSight = soldier;
                closestLOSDistance = distance;
            }
        }

        isFrenzied = closestInLineOfSight != null && closestLOSDistance <= frenziedDistance;
        targetedSoldier = closestInLineOfSight;
        if(closestInLineOfSight != null) {
            if(closestLOSDistance <= soldierTetherDistance) {
                return Vector2.zero;
            }
            var delta = closestInLineOfSight.transform.position - transform.position;
            return huntSoldiersStrength * delta.normalized * Mathf.Pow(huntSoldiersRange - delta.magnitude, huntSoldiersFalloff) / Mathf.Pow(huntSoldiersRange, huntSoldiersFalloff);
        }

        if (soldiersInRange.Count() == 0) {
            return Vector2.zero;
        }

        if(isIdle) {
            return Vector2.zero;
        }
        // We don't have any in line of sight, so should move along free wall vector towards closest distance
        float minDistance = Mathf.Infinity;
        int minIndex = -1;
        for(int i = 0; i < wallSensorCount; i++) {
            if(blockedWalls[i]) {
                continue;
            }
            Vector2 newPostion = (Vector2)transform.position + wallDirections[i] * roamingSpeed * Time.fixedDeltaTime;
            float newDistance = Vector2.Distance(newPostion, closestOverall.transform.position);
            if(newDistance < minDistance) {
                minDistance = newDistance;
                minIndex = i;
            }
        }
        if (minIndex == -1) {
            return Vector2.zero;
        }
        return huntSoldiersStrength * wallDirections[minIndex];
    }

    bool HasLOS(Vector2 position) {
        Physics2D.queriesHitTriggers = false;
        var v = position - (Vector2)transform.position;
        var hit = Physics2D.Raycast(transform.position, v, v.magnitude, ~LayerMask.GetMask("IgnoreEachother"));
        Physics2D.queriesHitTriggers = true;
        return hit.collider == null;
    }



    private void OnDrawGizmosSelected() {
        Gizmos.color = (Color.red + Color.yellow)/2.0f;
        for (int i = 0; i < wallSensorCount; i++) {
            float angle = Mathf.Deg2Rad * i * 360.0f / wallSensorCount;
            Vector2 v = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            Gizmos.DrawLine(transform.position, v * avoidWallsRange + (Vector2)transform.position);
        }
        Gizmos.color = Color.Lerp(Color.green, Color.white, 0.2f);
        Gizmos.DrawWireSphere(transform.position, avoidZombiesRange);
        Gizmos.color = Color.Lerp(Color.red, Color.white, 0.8f);
        Gizmos.DrawWireSphere(transform.position, huntSoldiersRange);
        Gizmos.color = Color.Lerp(Color.red, Color.white, 0.5f);
        Gizmos.DrawWireSphere(transform.position, frenziedDistance);
        Gizmos.color = Color.Lerp(Color.red, Color.white, 0.1f);
        Gizmos.DrawWireSphere(transform.position, soldierTetherDistance);
        if (targetedSoldier != null) {
            Gizmos.DrawLine(transform.position, targetedSoldier.transform.position);
        }
    }
}
