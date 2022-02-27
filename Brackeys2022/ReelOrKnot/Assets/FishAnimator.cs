using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAnimator : MonoBehaviour
{
    public Transform sprite;
    public FishController fishController;

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
        AnimateBody();
    }

    private void AnimateBody() {
        if (fishController.isOnBaitReel) {
            velocity = Camera.main.ScreenToWorldPoint(Input.mousePosition) - fishController.transform.position;
            float goalY = velocity.x > 0 ? 0.0f : 180.0f;
            float goalZ = Mathf.Atan2(velocity.y, Mathf.Abs(velocity.x)) * Mathf.Rad2Deg;

            sprite.localEulerAngles = new Vector3(0.0f, goalY, goalZ);
            position = sprite.position;
            sprite.position = fishController.rb.position;
        } else {
            position = Vector2.SmoothDamp(position, fishController.rb.position, ref velocity, damping);
            sprite.position = position;

            float goalY = velocity.x > 0 ? 0.0f : 180.0f;
            float goalZ = Mathf.Atan2(velocity.y, Mathf.Abs(velocity.x)) * Mathf.Rad2Deg;

            sprite.localEulerAngles = new Vector3(0.0f, Mathf.MoveTowards(sprite.localEulerAngles.y, goalY, turnSpeed * Time.deltaTime), goalZ);
        }
    }
}
