using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileMovementController : MonoBehaviour
{
    public Projectile projectile;
    public UnityEvent OnHit;
    public bool destroyOnHit;
    public Rigidbody2D rb;
    public Team sourceTeam;
    public AnimationCurve speedOverTime;
    public float speedMultiplier;

    private float _spawnTime;
    private void Start() {
        _spawnTime = Time.time;
        transform.localScale = Vector3.one * projectile.size;
    }

    private void FixedUpdate() {
        rb.velocity = transform.up * speedMultiplier * speedOverTime.Evaluate(Time.time - _spawnTime);
    }

    private void Update() {
        if(Time.time - _spawnTime >= projectile.maxTime) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        HealthController h = collision.GetComponentInParent<HealthController>();
        if (h == null || h.team != sourceTeam) {
            OnHit.Invoke();
            if (destroyOnHit) {
                Destroy(gameObject);
            }
            if (h != null) {
                h.TakeDamage(projectile.damage);
            }
        }
    }
}
