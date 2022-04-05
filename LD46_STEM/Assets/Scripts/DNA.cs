using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA : MonoBehaviour
{
    public GridElement element;

    public Animator animator;

    private DNATracker toPickUp;

    private void Update() {
        foreach(GridElement e in WorldGrid.Instance.ElementsAtPosition(element.position)) {
            DNATracker g = e.GetComponent<DNATracker>();
            if(g != null) {
                animator.SetTrigger("PickUp");
                toPickUp = g;
                return;
            }
        }
    }

    public void DoPickup() {
        toPickUp.currentDNA += 1;
        Destroy(gameObject);
    }
}
