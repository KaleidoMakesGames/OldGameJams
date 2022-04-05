using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementPad : MonoBehaviour
{
    public Collider2D placementCollider;

    public bool canUse;

    public bool CanPlace(CarryEnabler o) {
        return IsEmpty(o) && canUse;
    }

    public void Place(CarryEnabler o) {
        o.transform.parent = transform;
        o.transform.localEulerAngles = Vector3.zero;
        o.transform.localPosition = Vector3.zero;
    }

    public bool IsEmpty(CarryEnabler o) {
        ContactFilter2D filter = new ContactFilter2D();
        filter.NoFilter();

        List<Collider2D> hits = new List<Collider2D>();
        placementCollider.OverlapCollider(filter, hits);

        foreach (Collider2D hit in hits) {
            CarryEnabler carriable = hit.GetComponent<CarryEnabler>();
            if (carriable != null && carriable != o) {
                return false;
            }
        }
        return true;
    }
}
