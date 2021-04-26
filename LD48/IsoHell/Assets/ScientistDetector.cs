using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScientistDetector : MonoBehaviour
{
    public UnityEvent OnScientistsFound;
    public Dialogue OnFoundDialogue;
    public Dialogue OnNotSafeDialogue;
    private bool isSafe;
    public Animator doors;

    private void Update() {
        isSafe = FindObjectOfType<EnemyLogic>() == null;
        doors.SetBool("Lowered", isSafe);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.GetComponentInParent<PlayerMovementController>() != null) {
            var dc = FindObjectOfType<DialogueController>();
            if (!isSafe) {
                dc.EnqueueDialogue(OnNotSafeDialogue);
            } else {
                gameObject.SetActive(false);
                dc.EnqueueDialogue(OnFoundDialogue);
                dc.OnDialogueFinished.AddListener(delegate {
                    FindObjectOfType<ElevatorController>().OnScientistsFound.Invoke();
                    OnScientistsFound.Invoke();
                });
            }
        }
    }
}
