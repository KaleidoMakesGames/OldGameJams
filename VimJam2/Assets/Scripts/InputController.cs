using System.Collections;
using UnityEngine;

public class InputController : MonoBehaviour {
    [Header("Input Settings")]
    public bool hideCursor;
    public CursorLockMode cursorMode;

    [Header("References")]
    public TopDownMovementController movementController;
    public DashAttackController attackController;

    private Vector2 desiredDashPosition;

    private void Start() {
        Cursor.lockState = cursorMode;
        Cursor.visible = !hideCursor;
    }

    private void Update() {
        movementController.drive = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

        bool lastCharge = attackController.charge;
        attackController.charge = Input.GetButton("Charge") && !Input.GetButton("Parry");
        if(attackController.charge) {
            if (!lastCharge) {
                desiredDashPosition = movementController.characterMover.characterPosition;
            }
            desiredDashPosition += new Vector2(Input.GetAxis("Target X"), Input.GetAxis("Target Y"));
            attackController.desiredDashPosition = desiredDashPosition;
        }

        if (Input.GetButtonUp("Charge") && !Input.GetButton("Parry")) {
            attackController.TriggerDash();
        }
    }
}