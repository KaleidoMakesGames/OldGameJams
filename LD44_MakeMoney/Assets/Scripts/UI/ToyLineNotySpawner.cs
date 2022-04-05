using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyLineNotySpawner : MonoBehaviour
{
    public ToyLineNotificationController prefab;

    public void SpawnCorrect(float amount) {
        ToyLineNotificationController newController = Instantiate(prefab.gameObject, transform).GetComponent<ToyLineNotificationController>();
        newController.text = "+" + amount.ToString("C2");
        newController.isGood = true;
    }

    public void SpawnIncorrect(float amount) {
        ToyLineNotificationController newController = Instantiate(prefab.gameObject, transform).GetComponent<ToyLineNotificationController>();
        newController.text = "!!!";
        newController.isGood = false;
    }
}
