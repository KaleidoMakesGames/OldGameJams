    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashRenderer : MonoBehaviour
{
    [Header("Colors")]
    [ColorUsage(false)] public Color belowMinColor;
    [ColorUsage(false)] public Color chargingColor;

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
        maxRadiusViewer.gameObject.SetActive(dashController.isCharging);
        lineRenderer.enabled = dashController.isCharging;

        goalViewer.transform.position = dashController.desiredDashPosition;

        if (dashController.isCharging) {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, dashController.actualDashPosition);
            actualViewer.transform.position = dashController.actualDashPosition;
            radiusViewer.transform.localScale = Vector2.one * dashController.chargeRadius * 2.0f;
            maxRadiusViewer.transform.localScale = Vector2.one * dashController.dashMaxDistance * 2.0f;

            var color = chargingColor;
            if (dashController.actualRadius < dashController.dashMinDistance) {
                color = belowMinColor;
            }

            radiusViewer.color = new Color(color.r, color.g, color.b, radiusViewer.color.a);
            actualViewer.color = new Color(color.r, color.g, color.b, actualViewer.color.a);
            lineRenderer.startColor = actualViewer.color;
            lineRenderer.endColor = lineRenderer.startColor;
        }
    }
}
