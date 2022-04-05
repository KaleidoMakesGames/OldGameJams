using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ReelRenderer : MonoBehaviour
{
    public FishController controller;
    public LineRenderer lineRenderer;
    public Transform indicator;

    // Update is called once per frame
    void Update()
    {
        lineRenderer.enabled = controller.isOnBaitReel;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, new Vector2(controller.reelPosition.x, controller.surfaceHeight));
        lineRenderer.SetPosition(1, controller.rb.position);

        indicator.gameObject.SetActive(controller.isOnBaitReel);
        indicator.position = lineRenderer.GetPosition(0);
        indicator.localScale = Vector3.one * (controller.surfaceHeight - controller.reelPosition.y + controller.escapeDistance) * 2;
    }
}
