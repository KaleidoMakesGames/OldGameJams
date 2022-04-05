using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Cinemachine.CinemachineVirtualCamera))]
public class CameraPanner : MonoBehaviour {
    public LevelBoundsCalculator bounds;

    private Cinemachine.CinemachineVirtualCamera virtualCamera;

    private Vector3? downPosition;

    private void Awake() {
        virtualCamera = GetComponent<Cinemachine.CinemachineVirtualCamera>();
    }

    private void Update() {
        if(Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject()) {
            downPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    public void RelinkBounds() {
        bounds = FindObjectOfType<LevelBoundsCalculator>();
    }

    private void LateUpdate() {
        if(Input.GetMouseButton(1) && downPosition.HasValue) {
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            virtualCamera.transform.Translate(downPosition.Value - currentPosition);
        } else {
            downPosition = null;
        }

        if (bounds != null) {
            Vector3 constrained = bounds.bounds.ClosestPoint(virtualCamera.transform.position);
            constrained.z = virtualCamera.transform.position.z;
            virtualCamera.transform.position = constrained;
        }
    }
}
