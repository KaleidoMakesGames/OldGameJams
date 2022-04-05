using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnAroundCollider : MonoBehaviour {
    public bool turnAroundOnCollision;
    
    public UnityEvent ShouldTurnAround;

    private Collider2D myCollider;

    private void Start() {
        myCollider = GetComponent<Collider2D>();
    }

    private void FixedUpdate() {
        if(!turnAroundOnCollision) {
            Bounds colliderBounds = myCollider.bounds;
            bool seesGround = false;
            foreach (Collider2D hit in Physics2D.OverlapAreaAll(colliderBounds.min, colliderBounds.max)) {
                if (hit.gameObject.name == "Tilemap") {
                    seesGround = true;
                    break;
                }
            }
            if(!seesGround) {
                ShouldTurnAround.Invoke();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(!collision.isTrigger && turnAroundOnCollision && collision.GetComponent<GaiosChoiceApplier>() == null && collision.gameObject != transform.parent.gameObject) {
            ShouldTurnAround.Invoke();
        }
    }
}
