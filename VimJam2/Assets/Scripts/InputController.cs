using System.Collections;
using UnityEngine;

public class InputController : MonoBehaviour {
    [Header("Input Settings")]
    public bool hideCursor;
    public CursorLockMode cursorMode;

    [Header("References")]
    public TopDownMovementController movementController;
    public DashAttackController attackController;

    private void Start() {
        Cursor.lockState = cursorMode;
        Cursor.visible = !hideCursor;
    }

    private void Update() {
        movementController.drive = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

        attackController.charge = Input.GetButton("Charge") && !Input.GetButton("Parry");
        attackController.desiredDashPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetButtonUp("Charge") && !Input.GetButton("Parry")) {
            attackController.TriggerDash();
        }
    }
}