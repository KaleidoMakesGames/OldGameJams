using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResultsShowResetter : MonoBehaviour {
    public UnityEvent DoShow;
    public UnityEvent DoHide;

    public GameState showState;
    public GameStateTracker tracker;

    public bool hasResultsToShow { get; set; }

    private void Update() {
        if(!hasResultsToShow || tracker.currentState != showState) {
            DoHide.Invoke();
        }

        if(hasResultsToShow && tracker.currentState == showState) {
            DoShow.Invoke();
        }
    }
}
