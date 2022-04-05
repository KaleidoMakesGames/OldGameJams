using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePoint : MonoBehaviour
{
    public List<DialogueSystem.DialogueItem> itemsToQueue;

    public bool doBrake;
    public bool oneShot;

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.isTrigger) {
            return;
        }
        DialogueSystem d = collision.GetComponentInParent<DialogueSystem>();
        if(d != null) {
            foreach (var i in itemsToQueue) {
                d.Enqueue(i);
            }
            var c = d.GetComponent<CarController>();
            if (doBrake) {
                c.isRunning = false;
                d.Enqueue(new DialogueSystem.DialogueItem("", "", 0.001f, d.OnStart));
            }
            if(oneShot) {
                gameObject.SetActive(false);
            }
        }
    }
}
