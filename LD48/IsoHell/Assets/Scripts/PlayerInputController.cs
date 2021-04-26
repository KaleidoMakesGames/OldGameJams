using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public PlayerMovementController movementController;
    public AttackController attackController;

    private void Update() {
        Vector3 delta = Vector3.zero;
        delta += Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) ? Vector3.forward : Vector3.zero;
        delta += Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.UpArrow) ? Vector3.back: Vector3.zero;
        delta += Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) ? Vector3.left: Vector3.zero;
        delta += Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) ? Vector3.right: Vector3.zero;
        float angle = Vector3.SignedAngle(Vector3.forward, delta, Vector3.up);
        
        Vector3 cameraVector = Camera.main.transform.forward;
        cameraVector.y = 0.0f;

        if (delta == Vector3.zero) {
            cameraVector = Vector3.zero;
        } else {
            cameraVector = Quaternion.AngleAxis(angle, Vector3.up) * cameraVector.normalized;
        }

        movementController.drive = new Vector2(cameraVector.x, cameraVector.z);

        UpdateFireTarget();

        if(Input.GetMouseButton(0)) {
            attackController.TryFire();
        }
    }

    private void UpdateFireTarget() {
        var worldRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane originPlane = new Plane(Vector3.up, attackController.firePositions[0].position);
        float enter;
        if (originPlane.Raycast(worldRay, out enter)) {
            Vector3 position = worldRay.origin + worldRay.direction * enter;
            attackController.fireTarget = position;
        }
    }
}
