using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsUtility : MonoBehaviour {
    public static RaycastHit2D[] GravityRaycast(Vector2 pointStart, Vector2 initialVelocity, Vector2 pointEnd, float gravity, float deltaTime, float radius = 0.0f) {
        if(deltaTime == 0) {
            return null;
        }
        Vector2 currentPoint = pointStart;

        Vector2 currentVelocity = initialVelocity;
        
        Vector2 nextPoint = currentPoint + currentVelocity * deltaTime + (Vector2.down * gravity * Mathf.Pow(deltaTime, 2)) / (2.0f);

        bool travelingRight = initialVelocity.x > 0;

        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        while ((travelingRight && (nextPoint.x < pointEnd.x)) || (!travelingRight && (nextPoint.x > pointEnd.x))) {
            Vector2 direction = nextPoint - currentPoint;
            hits.AddRange(radius > 0 ? Physics2D.CircleCastAll(currentPoint, radius, direction.normalized, direction.magnitude) : Physics2D.LinecastAll(currentPoint, nextPoint));
            currentPoint = nextPoint;
            nextPoint = currentPoint + currentVelocity * deltaTime + (Vector2.down * gravity * Mathf.Pow(deltaTime, 2)) / (2.0f);
            currentVelocity += Vector2.down * gravity * deltaTime;
        }

        return hits.ToArray();
    }
}
