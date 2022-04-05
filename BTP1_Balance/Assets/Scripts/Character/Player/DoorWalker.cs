using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorWalker : MonoBehaviour {
    [System.Serializable]
    public class DoorEvent : UnityEvent<string> { }

    public DoorEvent OnEnteredDoor;

    private void OnTriggerEnter2D(Collider2D other) {
        Doorway doorway = other.GetComponent<Doorway>();
        if(doorway != null) {
            doorway.OnPlayerEntered.Invoke();
            OnEnteredDoor.Invoke(doorway.LevelToGoTo);
        }
    }
}
