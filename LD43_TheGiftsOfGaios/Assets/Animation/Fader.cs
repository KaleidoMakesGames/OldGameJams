using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Fader : MonoBehaviour {
    public UnityEvent fadedOut;
    public UnityEvent fadedIn;
    public void FadedIn() {
        fadedIn.Invoke();
    }

    public void FadedOut() {
        fadedOut.Invoke();
    }
}
