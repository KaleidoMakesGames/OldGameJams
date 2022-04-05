using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[ExecuteInEditMode]
public class MoverArrow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {
    [System.Serializable]
    public enum State { Normal, Hover, Pressed}
    public Image i;
    public Color normalColor;
    public Color hoverColor;
    public Color pressedColor;
    public State currentState;
    public UnityEvent OnPressed;
    public UnityEvent OnDown;

    private void Update() {
        i.color = currentState == State.Normal ? normalColor : i.color;
        i.color = currentState == State.Hover ? hoverColor: i.color;
        i.color = currentState == State.Pressed ? pressedColor : i.color;
    }

    public void OnPointerDown(PointerEventData eventData) {
        if(currentState == State.Hover) {
            OnDown.Invoke();
        }
        currentState = State.Pressed;
        OnPressed.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (currentState == State.Normal) {
            currentState = State.Hover;
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if(currentState == State.Hover) {
            currentState = State.Normal;
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        currentState = State.Hover;
    }
}
