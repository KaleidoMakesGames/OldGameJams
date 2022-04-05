using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialClicker : MonoBehaviour {
    public UnityEvent OnLeftClick;
    public UnityEvent OnRightUp;
    public UnityEvent OnEventHappened;

    private void Update() {
        if(Input.GetMouseButtonUp(0)) {
            OnLeftClick.Invoke();
        }
        if(Input.GetMouseButtonDown(1)) {
            OnRightUp.Invoke();
        }
    }

    public void EventHappened() {
        if (gameObject.activeInHierarchy) {
            OnEventHappened.Invoke();
        }
    }
}
