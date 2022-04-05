using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltFreezer : MonoBehaviour
{
    public void FreezeAllBelts() {
        foreach(BeltController controller in FindObjectsOfType<BeltController>()) {
            controller.minSpeed = 0.0f;
            controller.speed = 0.0f;
        }
    }
}
