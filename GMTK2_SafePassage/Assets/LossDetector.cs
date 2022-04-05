using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LossDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.isTrigger || collision.GetComponentInParent<LoseZone>() != null) {
            Checkpoint.ResetToLastCheckpoint(GetComponentInParent<CarController>());
        }
    }
}
