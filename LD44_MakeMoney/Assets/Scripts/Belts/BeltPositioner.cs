using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Collider2D))]
public class BeltPositioner : MonoBehaviour
{
    public bool snapTop;
    public BoxCollider2D beltCollider;

    private BoxCollider2D selfCollider;

    private void Update() {
        if (selfCollider == null) {
            selfCollider = GetComponent<BoxCollider2D>();
        }

        if (beltCollider != null) {
            Vector2 beltSpotLocal = new Vector2(0.0f, ((snapTop ? 1.0f : -1.0f) * (beltCollider.size.y/2.0f) + beltCollider.offset.y));
            Vector2 beltSpotWorld = beltCollider.transform.TransformPoint(beltSpotLocal);

            Vector2 selfSpotLocal = new Vector2(0.0f, ((snapTop ? 1.0f : -1.0f) * (selfCollider.size.y/2.0f) + selfCollider.offset.y));
            Vector2 selfSpotWorld = selfCollider.transform.TransformPoint(beltSpotLocal);

            Vector2 offset = beltSpotWorld - selfSpotWorld;
            if (offset.magnitude != 0) {
                transform.Translate(offset, Space.World);
                selfCollider.enabled = false;
                selfCollider.enabled = true;
            }
        }
    }
}
