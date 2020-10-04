using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritterAIController : MonoBehaviour
{
    public CharacterMovementController critterMovementController;

    public bool isBad;

    public float updateTime;

    private void OnEnable() {
        StartCoroutine(DirectionLoop());
    }

    private IEnumerator DirectionLoop() {
        while(true) {
            UpdateDirection();
            yield return new WaitForSeconds(updateTime);
        }
    }

    private void UpdateDirection() {
        critterMovementController.movementDrive = Random.insideUnitCircle.normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        critterMovementController.movementDrive = Vector2.Reflect(critterMovementController.movementDrive, collision.GetContact(0).normal);
    }
}
