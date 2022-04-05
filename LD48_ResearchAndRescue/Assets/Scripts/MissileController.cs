using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    public GameObject sourceObject;
    public float speed;
    public float damage;
    [HideInInspector] public Vector3 direction;
    public GameObject spawnOnHit;
    public Rigidbody rb;
    public float maxLifeTime;
    public float impulse;
    private float spawnTime;

    private void Start() {
        gameObject.layer = sourceObject.layer;
        spawnTime = Time.time;
    }

    private void Update() {
        transform.forward = direction;
        if(Time.time - spawnTime > maxLifeTime) {
            Destroy(gameObject);
        }
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
        var healthTracker = collision.collider.GetComponentInParent<HealthTracker>();
        if (healthTracker != null) {
            healthTracker.TakeDamage(damage);
        }
        var otherRB = collision.collider.GetComponentInParent<Rigidbody>();
        if(otherRB != null) {
            otherRB.AddForceAtPosition(-collision.contacts[0].normal * impulse, collision.contacts[0].point, ForceMode.Impulse);
        }
    }

    public void OnTriggerEnter(Collider other) {
        var healthTracker = other.GetComponentInParent<HealthTracker>();
        if (healthTracker != null) {
            Destroy(gameObject);
            healthTracker.TakeDamage(damage);
        }
    }
}
