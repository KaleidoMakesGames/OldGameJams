using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float groundForce;
    public float groundDrag;
    public float groundGravity;

    public float airDrag;
    public float airGravity;

    public float stopDrag;

    public Rigidbody2D rb;

    public bool isRunning { get; set; }

    public Checkpoint initialCheckpoint;

    private bool isGrounded;

    private Vector3 startPos;
    private Vector3 angles;

    private void Start() {
        startPos = transform.position;
        angles = transform.eulerAngles;

        if(initialCheckpoint != null) {
            initialCheckpoint.ResetToCheckpoint(this);
        }
    }

    private void FixedUpdate() {
        if(isGrounded) {
            rb.AddForce(Vector3.down * groundGravity);
            if (isRunning) {
                rb.AddForce(transform.right * groundForce);
            }
            rb.AddForce(-(Vector3)rb.velocity * (isRunning ? groundDrag : stopDrag));
        } else {
            rb.AddForce(Vector3.down * airGravity - (Vector3)rb.velocity * airDrag);
            rb.angularVelocity = 0.0f;
        }
        isGrounded = false;
    }

    private void OnCollisionStay2D(Collision2D collision) {
        isGrounded = true;
    }

    public void OnCheckpointReached() {
        var d = GetComponent<DialogueSystem>();
        d.Enqueue(new DialogueSystem.DialogueItem("Make it to a stopping point! We'll come back here if things go badly.", "", 2));
    }

    public void ResetToCheckpoint() {
        var d = GetComponent<DialogueSystem>();
        d.Enqueue(new DialogueSystem.DialogueItem("Ouch. That didn't go well. Let's give it another shot.", "Let's go!", 0, d.OnStart));
        Spawner.ClearSpawned();
    }

    public void FullReset() {
        transform.position = startPos;
        transform.eulerAngles = angles;
        var d = GetComponent<DialogueSystem>();
        d.Enqueue(new DialogueSystem.DialogueItem("Ouch. That didn't go well. Let's give it another shot.", "Let's go!", 0, d.OnStart));
        Spawner.ClearSpawned();
    }
}
