
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TestPhysics : MonoBehaviour
{
    public Vector2 direction;
    public float distance;
    public float radius;
    public Vector2 size;
    public float angle;

    private void Update() {
        Color c1 = Color.red;
        Color c2 = Color.black;
        var hits = KMGMovement2D.CharacterCollider.RoundedBoxCastAll(transform.position, size, radius, angle, direction, distance, ~0);
        int count = hits.Count();
        float n = 0;
        foreach(var hit in hits) {
            Color c = Color.Lerp(c1, c2, n);
            n += 1.0f / count;
            KMGMovement2D.DrawingUtilities.DrawRectangle((Vector2)transform.position + direction.normalized * hit.distance, size, radius, angle, c, true, 0);
            KMGMovement2D.DrawingUtilities.DrawCircle(hit.point, 0.1f, c, 0, 360, true, 0);
        }
    }

    private void OnDrawGizmos() {
        KMGMovement2D.DrawingUtilities.DrawRectangle(transform.position, size, radius, angle, Color.green, false, 0);
        KMGMovement2D.DrawingUtilities.DrawRectangle(direction.normalized*distance + (Vector2)transform.position, size, radius, angle, Color.green, false, 0);
    }
}
