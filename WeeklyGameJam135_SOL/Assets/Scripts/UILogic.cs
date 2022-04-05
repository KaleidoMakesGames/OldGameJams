using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UILogic : MonoBehaviour {
    [System.Serializable]
    public enum State { Menu, IdleOnSun, Escaping, About, Victory };
    public State currentState;

    public Cinemachine.CinemachineVirtualCamera menuCamera;
    public Cinemachine.CinemachineVirtualCamera cometCamera;
    public Cinemachine.CinemachineVirtualCamera aboutCamera;

    public GameObject menuUI;
    public GameObject aboutUI;
    public GameObject onSunUI;
    public GameObject escapeUI;
    public GameObject wishRenderer;
    public GameObject orbitUI;
    public GameObject victoryUI;

    public CometController c;

    private void Update() {
        menuCamera.Priority = currentState == State.Menu ? 1 : 0;
        aboutCamera.Priority = currentState == State.About ? 1 : 0;
        cometCamera.Priority = (currentState == State.IdleOnSun || currentState == State.Escaping || currentState == State.Victory) ? 1 : 0;
        menuUI.SetActive(currentState == State.Menu);
        aboutUI.SetActive(currentState == State.About);
        onSunUI.SetActive(currentState == State.IdleOnSun);
        escapeUI.SetActive(currentState == State.Escaping || currentState == State.IdleOnSun);
        victoryUI.SetActive(currentState == State.Victory);
        orbitUI.SetActive(c.currentState == CometController.State.InOrbit);

        c.enabled = currentState != State.Menu;
        if (currentState == State.IdleOnSun) {
            if (c.currentState == CometController.State.Launched) {
                currentState = State.Escaping;
            }
        }

        if (c.currentState == CometController.State.FreeAtLast) {
            currentState = State.Victory;
        }

        wishRenderer.gameObject.SetActive(c.currentState == CometController.State.Launched && c.tracker.currentNumberOfWishes > 0);
    }

    public void SetState(int s) {
        currentState = (State)s;
    }

    public void EnableRings() {
        Camera.main.cullingMask = ~0;
    }
}
