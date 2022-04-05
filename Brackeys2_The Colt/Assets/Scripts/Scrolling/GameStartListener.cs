using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStartListener : MonoBehaviour
{
    public UnityEvent OnStartedRun;
    public UnityEvent OnQuit;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            OnStartedRun.Invoke();
            this.enabled = false;
        }

        if(RunCountTracker.Instance.canGoHome && Input.GetKeyDown(KeyCode.LeftArrow)) {
            OnQuit.Invoke();
            this.enabled = false;
        }
    }
}
