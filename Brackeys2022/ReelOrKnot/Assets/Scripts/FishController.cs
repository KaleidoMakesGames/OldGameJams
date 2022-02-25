using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FishController : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed;
    public float energyCostPerUnitDistance;

    [Header("Eating")]
    public float eatRadius;
    public float maxEnergy;
    public float currentEnergy;

    [Header("Bait Reel System")]
    public float reelStrength;
    public AnimationCurve fishStrengthVsEnergyPercentage;
    public float escapeDistance;

    public bool isOnBaitReel;

    [Header("References")]
    public Rigidbody2D rb;

    private void Start() {
        currentEnergy = maxEnergy;
    }

    private void Update() {
        if (isOnBaitReel) {
        } else {
            if (Input.GetMouseButton(0)) {
                var food = LookForFood();
                if(food.Count > 0) {
                    Eat(food[0]);
                }
            }
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        float distanceTraveled = rb.velocity.magnitude * Time.fixedDeltaTime;
        currentEnergy = Mathf.Clamp(currentEnergy - energyCostPerUnitDistance * distanceTraveled, 0.0f, maxEnergy);
        DoMovement();
    }

    private void DoMovement() {
        Vector2 goalPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

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
        } else { 
            currentEnergy = Mathf.Clamp(currentEnergy + food.energy, 0, maxEnergy);
        }
        food.Eat();
    }
}
