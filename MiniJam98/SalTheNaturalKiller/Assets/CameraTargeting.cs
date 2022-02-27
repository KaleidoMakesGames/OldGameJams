using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargeting : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera vCamera;
    public float magnitude;
    // Update is called once per frame
    private void Start() {
    }
    void Update()
    {
        var transposer = vCamera.GetCinemachineComponent<Cinemachine.CinemachineFramingTransposer>();
        Vector2 viewportMousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition) - Vector3.one/2;
        Vector2 offsetCameraPosition = viewportMousePosition.normalized * magnitude;
        transposer.m_TrackedObjectOffset = offsetCameraPosition;
    }
}
