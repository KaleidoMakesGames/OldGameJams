using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Cinemachine.CinemachineVirtualCamera))]
public class CameraPlayerTracker : MonoBehaviour {
    [SerializeField] private Transform _playerToTrack;
    public Transform playerToTrack {
        get {
            return _playerToTrack;
        }
        set {
            _playerToTrack = value;
        }
    }


    private Cinemachine.CinemachineVirtualCamera virtualCamera;
    private void Awake() {
        virtualCamera = GetComponent<Cinemachine.CinemachineVirtualCamera>();    
    }

    private void Update() {
        virtualCamera.Follow = playerToTrack;
    }
}
