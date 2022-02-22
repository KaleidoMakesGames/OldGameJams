using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAnimator : MonoBehaviour
{
    public Transform sprite;
    public FishMovementController movementController;

    public float turnSpeed;
    public float damping;

    private Vector2 velocity;
    private Vector2 position;

    private void Start() {
        position = sprite.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        position = Vector2.SmoothDamp(position, movementController.rb.position, ref velocity, damping);
        sprite.position = position;

        float goalY = velocity.x > 0 ? 0.0f : 180.0f;
        float goalZ = Mathf.Atan2(velocity.y, Mathf.Abs(velocity.x)) * Mathf.Rad2Deg;

        sprite.localEulerAngles = new Vector3(0.0f, Mathf.MoveTowards(sprite.localEulerAngles.y, goalY, turnSpeed * Time.deltaTime), goalZ);
    }
}
