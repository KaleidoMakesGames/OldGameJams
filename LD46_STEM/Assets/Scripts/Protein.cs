using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protein : MonoBehaviour
{
    public GridElement element;

    public Animator animator;

    private ProteinGrabber toPickUp;

    private void Update() {
        foreach(GridElement e in WorldGrid.Instance.ElementsAtPosition(element.position)) {
            ProteinGrabber g = e.GetComponent<ProteinGrabber>();
            if(g != null) {
                animator.SetTrigger("PickUp");
                toPickUp = g;
                return;
            }
        }
    }

    public void DoPickup() {
        toPickUp.tracker.currentProtein += 1;
        Destroy(gameObject);
    }
}
