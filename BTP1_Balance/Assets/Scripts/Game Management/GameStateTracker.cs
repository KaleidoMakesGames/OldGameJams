using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateTracker : MonoBehaviour {
    public GameState currentState;

    public void GoToState(GameState state) {
        currentState = state;
    }
}
