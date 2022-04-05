using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFreezer : MonoBehaviour
{
    public void SetFreeze(bool frozen) {
        GetComponent<Carrier>().enabled = !frozen;
        GetComponent<MovementController>().enabled = !frozen;
    }
}
