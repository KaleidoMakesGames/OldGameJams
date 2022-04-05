using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="GoG/Weapons/Projectile Weapon")]
public class ProjectileWeapon : Weapon {
    public float lifetime;
    public float launchSpeed;
    public float gravity;

    public float hitRadius = 0.0f;

    public bool autoTarget;

    public float upAngleBounds;

    public float autoTargetDeltaTime;

    public ProjectileController projectileToFirePrefab;

    public override void Use(CharacterAttackController attackController, Vector2 target) {
        Team firingTeam = attackController.GetComponent<TeamAssigner>().team;

        Vector2 firePoint = attackController.attackBasePoint.transform.position;
        Vector2 delta = target - firePoint;

        Vector2 fireDirection = delta.normalized;

        bool isRight = delta.x > 0;

        if (autoTarget) {
            float sqrtTerm = Mathf.Pow(launchSpeed, 4) - gravity * (gravity * Mathf.Pow(delta.x, 2) + 2 * delta.y * Mathf.Pow(launchSpeed, 2));
            if (sqrtTerm >= 0) {
                float sol1 = Mathf.Atan((Mathf.Pow(launchSpeed, 2) + Mathf.Sqrt(sqrtTerm)) / (Mathf.Abs(delta.x * gravity)));
                float sol2 = Mathf.Atan((Mathf.Pow(launchSpeed, 2) - Mathf.Sqrt(sqrtTerm)) / (Mathf.Abs(delta.x * gravity)));

                Vector2 fireDirection1 = new Vector2(Mathf.Cos(sol1) * (isRight ? 1.0f : -1.0f), Mathf.Sin(sol1));
                Vector2 fireDirection2 = new Vector2(Mathf.Cos(sol2) * (isRight ? 1.0f : -1.0f), Mathf.Sin(sol2));

                bool canHit1 = true;
                bool canHit2 = true;

                // Check if we can hit the target without a collision:
                if (Vector2.Angle(attackController.transform.up, fireDirection1) * Mathf.Deg2Rad > upAngleBounds) {
                    canHit1 = false;
                } else {
                    foreach (RaycastHit2D hit in PhysicsUtility.GravityRaycast(firePoint, fireDirection1 * launchSpeed, target, gravity, autoTargetDeltaTime, hitRadius)) {
                        if (hit.collider.GetComponent<AutoProjectileBlocker>() != null) {
                            canHit1 = false;
                            break;
                        }
                    }
                }

                if (Vector2.Angle(attackController.transform.up, fireDirection2) * Mathf.Deg2Rad > upAngleBounds) {
                    canHit2 = false;
                } else {
                    foreach (RaycastHit2D hit in PhysicsUtility.GravityRaycast(firePoint, fireDirection2 * launchSpeed, target, gravity, autoTargetDeltaTime, hitRadius)) {
                        if (hit.collider.GetComponent<AutoProjectileBlocker>() != null) {
                            canHit2 = false;
                            break;
                        }
                    }
                }

                float? bestAngle = null;

                if (canHit1 && canHit2) {
                    float time1 = (launchSpeed * Mathf.Sin(sol1) + Mathf.Sqrt(Mathf.Pow(launchSpeed * Mathf.Sin(sol1), 2) + Mathf.Abs(2 * gravity * delta.y))) / gravity;
                    float time2 = (launchSpeed * Mathf.Sin(sol2) + Mathf.Sqrt(Mathf.Pow(launchSpeed * Mathf.Sin(sol2), 2) + Mathf.Abs(2 * gravity * delta.y))) / gravity;

                    bestAngle = (time1 > time2) ? sol2 : sol1;
                } else {
                    if (canHit1) {
                        bestAngle = sol1;
                    } else if (canHit2) {
                        bestAngle = sol2;
                    }
                }

                if (bestAngle.HasValue) {
                    fireDirection = new Vector2(Mathf.Cos(bestAngle.Value) * (isRight ? 1.0f : -1.0f), Mathf.Sin(bestAngle.Value));
                }
            }
        }

        if (Vector2.Angle(attackController.transform.up, fireDirection) * Mathf.Deg2Rad <= upAngleBounds) {
            ProjectileController newProjectile = Instantiate(projectileToFirePrefab.gameObject).GetComponent<ProjectileController>();
            newProjectile.Fire(firePoint, fireDirection, firingTeam, this);
        }
    }

    public override void DrawAttackGizmos(CharacterAttackController controller) {
        base.DrawAttackGizmos(controller);
        Gizmos.DrawLine(controller.transform.position, (Vector2)controller.transform.position + RotateVector(controller.transform.up, upAngleBounds));
        Gizmos.DrawLine(controller.transform.position, (Vector2)controller.transform.position + RotateVector(controller.transform.up, -upAngleBounds));
    }

    public Vector2 RotateVector(Vector2 v, float angle) {
        float _x = v.x * Mathf.Cos(angle) - v.y * Mathf.Sin(angle);
        float _y = v.x * Mathf.Sin(angle) + v.y * Mathf.Cos(angle);
        return new Vector2(_x, _y);
    }
}
