using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BeltEffector : MonoBehaviour
{
    public float speed;

    private void OnTriggerStay2D(Collider2D collision) {
        ToyController toy = collision.GetComponent<ToyController>();
        if(toy != null && !toy.isPickedUp) {
            // Move down line 
            toy.transform.Translate(transform.up * speed * Time.fixedDeltaTime, Space.World);
            // Center
            Vector2 pointLocal = transform.InverseTransformPoint(toy.transform.position);
            pointLocal.x = 0.0f;
            Vector2 pointWorld = transform.TransformPoint(pointLocal);
            Vector2 dirToCenter = (pointWorld - (Vector2)toy.transform.position).normalized;
            toy.transform.Translate(dirToCenter * speed * Time.fixedDeltaTime, Space.World);
        }
    }
}
