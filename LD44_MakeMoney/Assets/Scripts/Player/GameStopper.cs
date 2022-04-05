using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStopper : MonoBehaviour
{
    public UnityEvent OnStopGame;

    public void StopGame() {
        OnStopGame.Invoke();
    }
}
