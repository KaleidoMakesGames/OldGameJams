    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashRenderer : MonoBehaviour
{
    [Header("Colors")]
    [ColorUsage(false)] public Color belowMinColor;
    [ColorUsage(false)] public Color chargingColor;
    [ColorUsage(false)] public Color maxColor;

    [Header("References")]
    public DashAttackController dashController;
    public SpriteRenderer radiusViewer;
    public SpriteRenderer actualViewer;
    public SpriteRenderer goalViewer;
    public SpriteRenderer maxRadiusViewer;
    

    public LineRenderer lineRenderer;

    // Update is called once per frame
    void Update()
    {
        radiusViewer.gameObject.SetActive(dashController.isCharging);
        actualViewer.gameObject.SetActive(dashController.isCharging);
        goalViewer.gameObject.SetActive(dashController.isCharging);
        maxRadiusViewer.gameObject.SetActive(dashController.isCharging);
        lineRenderer.enabled = dashController.isCharging;

        if (dashController.isCharging) {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, dashController.actualDashPosition);
            goalViewer.transform.position = dashController.desiredDashPosition;
            actualViewer.transform.position = dashController.actualDashPosition;
            radiusViewer.transform.localScale = Vector2.one * dashController.chargeRadius * 2.0f;
            maxRadiusViewer.transform.localScale = Vector2.one * dashController.dashMaxDistance * 2.0f;

            var color = chargingColor;
            if (dashController.actualRadius < dashController.dashMinDistance) {
                color = belowMinColor;
            } else if(dashController.chargeRadius == dashController.dashMaxDistance) {
                color = maxColor;
            }

            radiusViewer.color = new Color(color.r, color.g, color.b, radiusViewer.color.a);
            actualViewer.color = new Color(color.r, color.g, color.b, actualViewer.color.a);
            lineRenderer.startColor = actualViewer.color;
            lineRenderer.endColor = lineRenderer.startColor;
        }
    }
}
