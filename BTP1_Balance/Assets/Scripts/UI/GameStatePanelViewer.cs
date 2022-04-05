using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatePanelViewer : MonoBehaviour {
    public GameState gameState;
    public GameStateTracker tracker;

    private void Awake() {
        gameObject.SetActive(true);
    }

    private void Update() {
        if(tracker != null) {
            foreach (Transform t in transform) {
                t.gameObject.SetActive(tracker.currentState == gameState);
            }
        }
    }
}
