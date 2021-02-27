using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ProjectileRenderer : MonoBehaviour
{
    public ProjectileMovementController movementController;

    public SpriteRenderer projectileRenderer;

    private void Update() {
        if (projectileRenderer != null && movementController != null && movementController.projectile != null && movementController.sourceTeam != null) {
            projectileRenderer.sprite = movementController.projectile.sprite;
            projectileRenderer.color = movementController.sourceTeam == null ? Color.white : movementController.sourceTeam.color;
            if(movementController.projectile.alignUp) {
                projectileRenderer.transform.up = Vector2.up;
            }
        }
    }
}
