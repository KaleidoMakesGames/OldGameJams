using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Weapon weapon;

    public ProjectileMovementController projectilePrefab;

    [ReadOnly] public int bulletsInClip;

    public bool triggerPressed;

    public Vector2 fireDirection;

    public Vector2 firePoint;

    public HealthController healthController;

    private float _lastFireTime;

    private bool isBusy;

    private void Start() {
        bulletsInClip = weapon.clipSize;
    }

    private void Update() {
        if(isBusy) {
            return;
        }

        if (triggerPressed) {
            if (bulletsInClip > 0 && Time.time - _lastFireTime >= weapon.fireDelay) {
                StartCoroutine(FireBurst());
            }
        }
    }
    
    private IEnumerator FireBurst() {
        isBusy = true;
        int bulletsToFire = Mathf.Min(weapon.projectilesPerBurst, bulletsInClip);
        for (int i = 0; i < bulletsToFire; i++) {
            Fire();
            yield return new WaitForSeconds(weapon.burstDelay);
        }
        isBusy = false;
    }

    public void Fire() {
        ProjectileMovementController projectile = Instantiate(projectilePrefab.gameObject).GetComponent<ProjectileMovementController>();
        projectile.projectile = weapon.projectileToShoot;
        projectile.speedMultiplier = weapon.speedMultiplier;
        projectile.speedOverTime = weapon.speedOverTime;
        projectile.sourceTeam = healthController.team;
        projectile.transform.position = transform.TransformPoint(firePoint);
        projectile.transform.up = fireDirection;
        bulletsInClip--;
        _lastFireTime = Time.time;
    }

    private void OnDrawGizmosSelected() {
        if (healthController == null || healthController.team == null) {
            Gizmos.color = Color.white;
        } else {
            Gizmos.color = healthController.team.color;
        }
        Gizmos.DrawWireSphere(transform.TransformPoint(firePoint), 0.1f);
    }
}
