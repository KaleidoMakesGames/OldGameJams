using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Historian {
    public MonoBehaviour objectToTrack;
    public List<object> objectStates;

    public abstract object PrepareState();
    public abstract void SetState(object state, float transitionTime);

    public bool isTransitioning;

    public Historian(MonoBehaviour o) {
        objectToTrack = o;
        isTransitioning = false;
        objectStates = new List<object>();
    }

    public void Record() {
        objectStates.Add(PrepareState());
    }

    public void DumpAfterInclusive(int index) {
        int numberToRemove = objectStates.Count - index;
        if (numberToRemove > 0) {
            objectStates.RemoveRange(index, numberToRemove);
        }
    }

    public void Seek(int toState, float transitionTime) {
        if (toState < 0 || toState >= objectStates.Count) {
            Debug.LogError("Cannot go to state " + toState + ". The object" + objectToTrack + " only has " + objectStates.Count + " states recorded.");
        }
        isTransitioning = true;
        SetState(objectStates[toState], transitionTime);
    }
}
