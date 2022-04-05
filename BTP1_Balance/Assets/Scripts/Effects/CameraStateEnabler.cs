using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Cinemachine.CinemachineVirtualCamera))]
public class CameraStateEnabler : MonoBehaviour {
    public GameState onState;
    public GameStateTracker tracker;

    private Cinemachine.CinemachineVirtualCamera virtualCamera;

    private void Awake() {
        virtualCamera = GetComponent<Cinemachine.CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update () {
        virtualCamera.enabled = (tracker == null || tracker.currentState == onState);
	}
}
