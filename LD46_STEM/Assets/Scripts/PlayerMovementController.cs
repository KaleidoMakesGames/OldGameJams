using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public MovementController movementController;

    private void Update() {
        Vector2Int movementDirection = Vector2Int.zero;
        movementDirection = movementDirection + (Input.GetKey(KeyCode.W) ? Vector2Int.up : Vector2Int.zero);
        movementDirection = movementDirection + (Input.GetKey(KeyCode.S) ? Vector2Int.down: Vector2Int.zero);
        movementDirection = movementDirection + (Input.GetKey(KeyCode.A) ? Vector2Int.left : Vector2Int.zero);
        movementDirection = movementDirection + (Input.GetKey(KeyCode.D) ? Vector2Int.right : Vector2Int.zero);

        movementController.drive = movementDirection;
    }
}
