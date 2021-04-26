using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    public GameObject sourceObject;
    public float speed;
    [HideInInspector] public Vector3 direction;
    public GameObject spawnOnHit;
    public Rigidbody rb;

    private void Start() {
        gameObject.layer = sourceObject.layer;    
    }

    private void FixedUpdate() {
        rb.velocity = direction.normalized * speed;
    }

    public void OnCollisionEnter(Collision collision) {
        Destroy(gameObject);
        if(spawnOnHit != null) {
            var spawned = Instantiate(spawnOnHit).transform;
            spawned.position = collision.contacts[0].point;
            spawned.up = collision.contacts[0].normal;
        }
    }
}
