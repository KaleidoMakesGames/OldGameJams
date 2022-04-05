using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ToyController : MonoBehaviour
{
    public ToyType toyType;

    private Collider2D toyCollider;

    private Collider2D[] overlappingColliders;

    public bool isPickedUp { get; private set; }

    public bool canPutDown { get; private set; }
    
    public void PickUp() {
        isPickedUp = true;
        Debug.Log("Picked up.");
    }

    public void PutDown() {
        isPickedUp = false;
        Debug.Log("Put down.");
        GetComponent<Rigidbody2D>().WakeUp();
    }

    private void Awake() {
        toyCollider = GetComponent<Collider2D>();
        overlappingColliders = new Collider2D[10];
    }

    private void Update() {
        if(isPickedUp) {
            UpdatePutDown();
        }
    }

    private void UpdatePutDown() {
        canPutDown = false;

        ContactFilter2D filter = new ContactFilter2D();
        filter.NoFilter();
        int numOverlap = Physics2D.OverlapCollider(toyCollider, filter, overlappingColliders);
        for (int i = 0; i < numOverlap; i++) {
            ToyPutDownRegion regionInfo = overlappingColliders[i].GetComponent<ToyPutDownRegion>();
            if (regionInfo != null) {
                if (regionInfo.canPutDownHere) {
                    canPutDown = true;
                } else {
                    canPutDown = false;
                    return;
                }
            }
        }
    }
}
