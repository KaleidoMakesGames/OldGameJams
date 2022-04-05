using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraGaiosFinder : MonoBehaviour {
    private CinemachineVirtualCamera vCamera;

    public float wideZoom;
    public float closeZoom;

    private void Start() {
        vCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update() {
        if(vCamera.Follow == null) {
            vCamera.Follow = PlayerChoices.FindGaiosComponent<MonoBehaviour>().transform;
        }

        vCamera.m_Lens.OrthographicSize = PlayerChoices.Instance.hasFullVision ? wideZoom : closeZoom;
    }
}
