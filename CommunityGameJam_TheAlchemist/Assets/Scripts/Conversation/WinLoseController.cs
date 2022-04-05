using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WinLoseController : MonoBehaviour
{
    [System.Serializable]
    public enum State { Nothing, Win, Lose }

    public State state;

    public UnityEvent OnWin;
    public UnityEvent OnLose;

    private void Awake() {
        state = State.Nothing;
    }

    public void DoCheck() {
        if(state == State.Win) {
            OnWin.Invoke();
        }
        if(state == State.Lose) {
            OnLose.Invoke();
        }
    }

    public void SetWin() {
        state = State.Win;
    }

    public void SetLose() {
        state = State.Lose;
    }
}
