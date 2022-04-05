using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitBody : MonoBehaviour
{
    public float radius {
        get {
            return transform.lossyScale.x/2.0f;
        }
    }

    public RadiusRenderer rr;
    
    private static CometController comet;
    private static CircleCollider2D cometRB;

    private void Awake() {
        if (comet == null) {
            comet = FindObjectOfType<CometController>();
            cometRB = comet.GetComponent<CircleCollider2D>();
        }
    }

    private void Update() {
        if(Vector2.Distance(comet.transform.position, transform.position) <= radius + cometRB.radius) {
            if(comet.currentState == CometController.State.Launched) {
                comet.DoLand(this);
            }
        }
    }
}
