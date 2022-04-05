using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(GameStateTracker))]
public class GameStateTransitionTimer : MonoBehaviour {
    public GameState startTimerState;
    public GameState endTimerState;

    public float timeToTransition;

    public float timeLeft { get; private set;}

    public UnityEvent OnTimerEnded;

    private IEnumerator timer;

    private GameStateTracker tracker;

    private void Awake() {
        tracker = GetComponent<GameStateTracker>();
        timer = null;
    }

    private void Update() {
        if(tracker.currentState == startTimerState && timer == null) {
            timer = RunTimer();
            StartCoroutine(timer);
        }
    }

    private IEnumerator RunTimer() {
        float startTime = Time.time;
        while(Time.time - startTime < timeToTransition) {
            timeLeft = timeToTransition - (Time.time - startTime);
            yield return null;
        }

        if(tracker != null) {
            tracker.currentState = endTimerState;
        }
        timer = null;

        OnTimerEnded.Invoke();
    }
}
