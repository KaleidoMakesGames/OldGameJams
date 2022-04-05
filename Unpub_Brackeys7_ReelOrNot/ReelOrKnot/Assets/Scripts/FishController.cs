using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class FishController : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed;
    public float energyCostPerUnitDistance;
    public float energyCostOverTime;

    [Header("Eating")]
    public float eatRadius;
    public float maxEnergy;
    public float currentEnergy;

    [Header("Bait Reel System")]
    public float reelStrength;
    public AnimationCurve fishPulseStrengthVersusEnergy;
    public float maxPulseStrength;
    public float energyCostPerPulse;
    public float escapeDistance;
    public float escapeForce;
    public float surfaceHeight;
    public float catchDistance;

    public bool isOnBaitReel;
    public Vector2 reelPosition;

    [System.Serializable]
    public struct Events {
        public UnityEvent OnDie;
        public UnityEvent OnCaught;
        public UnityEvent OnEat;
        public UnityEvent OnReel;
        public UnityEvent OnEscaped;
    }
    public Events events;

    [Header("References")]
    public Rigidbody2D rb;

    private bool isEscaping;

    private void Start() {
        currentEnergy = maxEnergy;
    }

    private void Update() {
        if(currentEnergy <= 0) {
            events.OnDie.Invoke();
            enabled = false;
            isOnBaitReel = false;
        }
        if (isEscaping) {
            if(rb.velocity.magnitude <= movementSpeed) {
                isEscaping = false;
            }
            return;
        }
        if (isOnBaitReel) {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) {
                Vector2 goalPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 pushForce = Vector2.ClampMagnitude(goalPosition - rb.position, 1.0f) * maxPulseStrength * fishPulseStrengthVersusEnergy.Evaluate(currentEnergy/maxEnergy);
                currentEnergy = Mathf.Clamp(currentEnergy - energyCostPerPulse, 0.0f, maxEnergy);
                rb.AddForce(pushForce, ForceMode2D.Impulse);
            }
        } else {
            currentEnergy = Mathf.Clamp(currentEnergy - energyCostOverTime * Time.deltaTime, 0.0f, maxEnergy);
            if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space)) {
                var food = LookForFood();
                if(food.Count > 0) {
                    Eat(food[0]);
                }
            }
        }
    }

    private void Escape() {
        isEscaping = true;
        Vector3 pullPoint = new Vector2(reelPosition.x, surfaceHeight);
        rb.AddForce((transform.position - pullPoint).normalized * escapeForce, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isEscaping) {
            return;
        }
        if (isOnBaitReel) {
            DoReelMovement();
        } else {
            DoFreeMovement();
        }
    }

    private void DoReelMovement() {
        Vector3 pullPoint = new Vector2(reelPosition.x, surfaceHeight);
        rb.AddForce((pullPoint - transform.position).normalized * reelStrength);

        float reelLength = surfaceHeight - reelPosition.y;
        if (Vector2.Distance(pullPoint, rb.position) >= (reelLength + escapeDistance)) {
            isOnBaitReel = false;
            Escape();
            return;
        }

        if (Vector2.Distance(pullPoint, rb.position) <= catchDistance) {
            events.OnCaught.Invoke();
            isOnBaitReel = false;
            enabled = false;
        }
    }

    private void DoFreeMovement() {
        Vector2 goalPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float distanceTraveled = rb.velocity.magnitude * Time.fixedDeltaTime;
        currentEnergy = Mathf.Clamp(currentEnergy - energyCostPerUnitDistance * distanceTraveled, 0.0f, maxEnergy);

        rb.velocity = Vector2.ClampMagnitude(goalPosition - rb.position, 1.0f) * movementSpeed;
    }
    private List<FoodController> LookForFood() {
        var foodInRange = new List<FoodController>();
        foreach (var hit in Physics2D.OverlapCircleAll(transform.position, eatRadius)) {
            var fc = hit.GetComponentInParent<FoodController>();
            if (fc != null) {
                foodInRange.Add(fc);
            }
        }
        foodInRange.OrderBy(x => Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), x.transform.position));
        return foodInRange;
    }
    private void Eat(FoodController food) {
        if (food.isBait) {
            isOnBaitReel = true;
            reelPosition = food.transform.position;
        } else { 
            currentEnergy = Mathf.Clamp(currentEnergy + food.energy, 0, maxEnergy);
        }
        food.Eat();
    }

    private void OnDrawGizmos() {
        if(isOnBaitReel) {
            Vector3 pullPoint = new Vector2(reelPosition.x, surfaceHeight);
            float reelLength = surfaceHeight - reelPosition.y;
            Gizmos.DrawWireSphere(pullPoint, reelLength + escapeDistance);
        }
    }
}
