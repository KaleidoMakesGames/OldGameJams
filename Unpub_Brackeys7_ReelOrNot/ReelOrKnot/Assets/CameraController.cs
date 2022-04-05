using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public FishController fishController;
    public Cinemachine.CinemachineVirtualCamera fishCam;
    public Cinemachine.CinemachineVirtualCamera baitCam;

    // Update is called once per frame
    void Update()
    {
        fishCam.enabled = !fishController.isOnBaitReel;
        baitCam.enabled = fishController.isOnBaitReel;
    }
}
