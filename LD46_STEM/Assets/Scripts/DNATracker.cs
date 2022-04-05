using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNATracker : MonoBehaviour { 
    private int _currentDNA;
    public int currentDNA {
        get {
            return _currentDNA;
        }
        set {
            _currentDNA = value;
            if(currentDNA >= numberToWin) {
                OnWin.Invoke();
            }
        }
    }

    public int numberToWin;

    public UnityEngine.Events.UnityEvent OnWin;

    private void Awake() {
        currentDNA = 0;
    }
}
