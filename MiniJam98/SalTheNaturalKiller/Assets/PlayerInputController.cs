using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public KMGMovement2D.TopDownMovementController movementController;

    // Update is called once per frame
    void Update()
    {
        movementController.drive = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
}
