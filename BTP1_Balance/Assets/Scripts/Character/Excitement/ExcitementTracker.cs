using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExcitementTracker : MonoBehaviour {
    public float maxExcitement;
    public float currentExcitement;

    public float excitementDecayPerSecond;

    public UnityEvent OnLostExcitement;

    private void Update() {
        currentExcitement = Mathf.Clamp(currentExcitement - excitementDecayPerSecond * Time.deltaTime, 0.0f, maxExcitement);

        if(currentExcitement <= 0) {
            OnLostExcitement.Invoke();
        }
    }

    public void GainExcitement(float amount) {
        currentExcitement = Mathf.Clamp(currentExcitement + amount, 0.0f, maxExcitement);
    }
}
