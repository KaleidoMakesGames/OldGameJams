using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HoverableUI : MonoBehaviour {
    public UnityEvent OnMouseIsOver;
    public UnityEvent OnMouseNotOver;

    private bool isMouseOver = false;

    private void Update() {
        if(isMouseOver) {
            OnMouseIsOver.Invoke();
        } else {
            OnMouseNotOver.Invoke();
        }
    }

    private void LateUpdate() {
        isMouseOver = false;
    }

    private void OnMouseOver() {
        isMouseOver = true;
    }
}
