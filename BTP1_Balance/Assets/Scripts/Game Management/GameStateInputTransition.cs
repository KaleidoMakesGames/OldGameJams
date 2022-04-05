using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(GameStateTracker))]
public class GameStateInputTransition : MonoBehaviour {
    public GameState startTimerState;
    public GameState endTimerState;

    public UnityEvent OnTimerEnded;

    private GameStateTracker tracker;

    private void Awake() {
        tracker = GetComponent<GameStateTracker>();
    }

    private void Update() {
        if(Input.GetKeyUp(KeyCode.Space)) {
            if (tracker.currentState == startTimerState) {
                tracker.currentState = endTimerState;
                OnTimerEnded.Invoke();
            }
        }
        Time.timeScale = Input.GetKey(KeyCode.Space) ? 4.0f : 1.0f;
    }
}
