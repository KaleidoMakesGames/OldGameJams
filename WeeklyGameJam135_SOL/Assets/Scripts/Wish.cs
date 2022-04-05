using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wish : MonoBehaviour {
    private static CometController comet;
    private static CircleCollider2D cometRB;

    public Canvas c;

    public float radius = 1.0f;

    private void Awake() {
        if (comet == null) {
            comet = FindObjectOfType<CometController>();
            cometRB = comet.GetComponent<CircleCollider2D>();
        }
    }

    private void Update() {
        if (Vector2.Distance(comet.transform.position, transform.position) <= radius) {
            comet.tracker.AddWish();
            Destroy(gameObject);
        }
        c.transform.up = comet.cometCam.transform.up;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
