using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public PathDrawer pathDrawer;
    public CharacterMovementController playerMovementController;
    public PathController snarePath;
    public PathController freezePath;

    public enum TraceState { None, Snare, Freeze}
    [ReadOnly] public TraceState currentState;

    // Update is called once per frame
    void Update() {
        switch(currentState) {
            case TraceState.Freeze:
                pathDrawer.prefabPath = freezePath;
                pathDrawer.SetCurrentPoint(transform.position);
                if (!Input.GetButton("Freeze")) {
                    currentState = TraceState.None;
                }
                break;
            case TraceState.Snare:
                pathDrawer.prefabPath = snarePath;
                pathDrawer.SetCurrentPoint(transform.position);
                if(!Input.GetButton("Snare")) {
                    currentState = TraceState.None;
                }
                break;
            case TraceState.None:
                if(Input.GetButton("Snare")) {
                    currentState = TraceState.Snare;
                }
                if (Input.GetButton("Freeze")) {
                    currentState = TraceState.Freeze;
                }
                pathDrawer.ClearPath();
                break;
        }

        Vector2 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        playerMovementController.movementDrive = Vector2.ClampMagnitude(currentMousePosition - (Vector2)transform.position, 1.0f);
    }
}
