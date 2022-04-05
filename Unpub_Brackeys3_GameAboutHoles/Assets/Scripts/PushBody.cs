using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PushBody : MonoBehaviour
{
    public Collider colliderObject;

    private Rigidbody _rb;
    private float freezeHeight;

    private bool isFrozen;

    // TODO: Inefficient
    public static List<PushBody> bodies {
        get {
            return new List<PushBody>(FindObjectsOfType<PushBody>());
        }
    }

    public static void FreezeAll() {
        if(bodies == null) {
            return;
        }
        foreach(PushBody f in bodies) {
            f.Freeze();
        }
    }

    public static void ThawAll() {
        foreach (PushBody f in bodies) {
            f.Thaw();
        }
    }

    private void Awake() {
        _rb = GetComponent<Rigidbody>();
        freezeHeight = colliderObject.transform.localPosition.y;
    }

    private void Start() {
        Thaw();
    }

    private void FixedUpdate() {
        if(isFrozen) {
            _rb.velocity = Vector3.zero;
        } else {
            _rb.velocity = new Vector3(0.0f, _rb.velocity.y, 0.0f);
        }
    }

    public void Freeze() {
        isFrozen = true;
        _rb.velocity = Vector3.zero;
        _rb.useGravity = false;
        freezeHeight = colliderObject.transform.localPosition.y;
        colliderObject.transform.localPosition = new Vector3(colliderObject.transform.localPosition.x, freezeHeight+0.05f, colliderObject.transform.localPosition.z);
        Physics.SyncTransforms();
    }

    public void Thaw() {
        isFrozen = false;
        transform.localPosition = new Vector3(Mathf.Round(transform.localPosition.x),
            Mathf.Round(transform.localPosition.y),
            Mathf.Round(transform.localPosition.z));
        _rb.velocity = Vector3.zero;
        _rb.useGravity = true;
        colliderObject.transform.localPosition = new Vector3(colliderObject.transform.localPosition.x, freezeHeight, colliderObject.transform.localPosition.z);
        Physics.SyncTransforms();
    }
}
