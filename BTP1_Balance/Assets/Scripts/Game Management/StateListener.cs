using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StateListener : MonoBehaviour {
    public GameState onState;
    public GameStateTracker tracker;

    public UnityEvent OnEventOn;
    public UnityEvent OnEventOff;

    private void Start() {
        if (tracker != null) {
            tracker.StartCoroutine(UpdateMe());
        } else {
            tracker = FindObjectOfType<GameStateTracker>();
        }

    }

    private void DoUpdate() {
        if (tracker == null) {
            return;
        }

        if (tracker.currentState == onState) {
            OnEventOn.Invoke();
        } else {
            OnEventOff.Invoke();
        }
    }

    private IEnumerator UpdateMe() {
        while (true) {
            DoUpdate();
            yield return null;
        }
    }
}
