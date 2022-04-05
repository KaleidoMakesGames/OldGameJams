using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapFocuser : MonoBehaviour
{
    public Camera mmCamera;

    private void LateUpdate() {
        Bounds world = WorldGrid.Instance.worldBounds;
        
        world.Expand(1.0f);

        mmCamera.transform.position = new Vector3(world.center.x, world.center.y, mmCamera.transform.position.z);
        if(world.extents.x > world.extents.y) {
            mmCamera.orthographicSize = world.extents.x / mmCamera.aspect;
        } else {
            mmCamera.orthographicSize = world.extents.y;
        }
    }
}
