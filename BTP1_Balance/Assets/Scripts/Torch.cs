using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour {
    public bool onByDefault = true;

    private void Start() {
        SetOn(onByDefault);
    }

    public void SetOn(bool isOn) {
        foreach(Transform t in transform) {
            t.gameObject.SetActive(isOn);
        }
    }
}
