using System.Collections;
using UnityEngine;

public class InputController : MonoBehaviour {
    [Header("References")]
    public TopDownMovementController movementController;
    public DashAttackController attackController;

    private void Start() {
        UseGameCursor();
    }

    private void Update() {
        movementController.drive = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

        attackController.charge = Input.GetButton("Charge") && !Input.GetButton("Parry");
        attackController.desiredDashPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetButtonUp("Charge") && !Input.GetButton("Parry")) {
            attackController.TriggerDash();
        }
    }

    public void UseSystemCursor() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void UseGameCursor() {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
}