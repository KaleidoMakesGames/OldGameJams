using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileController : MonoBehaviour {
    public Team firingTeam { get; private set; }
    private Rigidbody2D rb;
    private float spawnTime;

    private ProjectileWeapon weaponInfo;

	// Use this for initialization
	void Awake () {
        rb = GetComponent<Rigidbody2D>();	
	}
	
	void FixedUpdate () {
        rb.velocity += Vector2.down * weaponInfo.gravity * Time.fixedDeltaTime;
        transform.right = rb.velocity.normalized;
        if(Time.fixedTime - spawnTime >= weaponInfo.lifetime) {
            Destroy(gameObject);
        }
	}

    public void Fire(Vector2 fromWhere, Vector2 direction, Team firingTeam, ProjectileWeapon weaponInfo) {
        transform.position = fromWhere;
        transform.right = direction.normalized;
        spawnTime = Time.time;

        this.firingTeam = firingTeam;
        this.weaponInfo = weaponInfo;
        rb.velocity = direction.normalized * weaponInfo.launchSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        TeamAssigner teamAssigner = collider.GetComponent<TeamAssigner>();
        ProjectileController projectile = collider.GetComponent<ProjectileController>();

        if (projectile != null) {
            if (projectile.firingTeam != firingTeam) {
                Destroy(projectile.gameObject);
                Destroy(gameObject);
                return;
            }
        }

        if (collider.isTrigger || teamAssigner != null && firingTeam == teamAssigner.team) {
            return;
        } else {
            HealthTracker healthTracker = collider.GetComponent<HealthTracker>();
            if (healthTracker != null) {
                healthTracker.TakeDamage(weaponInfo.damage);
            }
            Destroy(gameObject);
        }
    }
}
