    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashRenderer : MonoBehaviour
{
    [Header("Colors")]
    public Color idleColor;
    public Color burnoutColor;
    public Color chargingColor;
    public Color cooldownColor;

    [Header("References")]
    public DashAttackController dashController;
    public LineRenderer currentDistanceRenderer;
    public Transform maxDistanceRenderer;
    public SpriteRenderer dashDestinationIndicator;
    public ArrowRenderer arrowRenderer;

    public Transform cursor;
    public Image cooldownDial;

    // Update is called once per frame
    void Update()
    {
        bool showCharge = dashController.currentState == DashAttackController.State.CHARGING;
        currentDistanceRenderer.gameObject.SetActive(showCharge);
        dashDestinationIndicator.gameObject.SetActive(showCharge);
        maxDistanceRenderer.gameObject.SetActive(showCharge);
        arrowRenderer.gameObject.SetActive(showCharge);

        cursor.transform.position = dashController.desiredDashPosition;
        cursor.transform.up = cursor.transform.position - dashController.transform.position;
        cooldownDial.transform.eulerAngles = Vector3.zero;

        var color = dashController.currentState == DashAttackController.State.IDLE ? idleColor :
            dashController.currentState == DashAttackController.State.CHARGING ? chargingColor :
            dashController.currentState == DashAttackController.State.BURNOUT ? burnoutColor :
            cooldownColor;
        foreach (var spriteRenderer in cursor.GetComponentsInChildren<SpriteRenderer>()) {
            spriteRenderer.color = color;
            //spriteRenderer.enabled = !showCharge;
        }

        if (dashController.currentState == DashAttackController.State.CHARGING) {
            Vector2 delta = dashController.actualDashPosition - (Vector2)transform.position;
            arrowRenderer.length = delta.magnitude;
            arrowRenderer.transform.up = delta;
            dashDestinationIndicator.transform.position = dashController.actualDashPosition;

            currentDistanceRenderer.SetPosition(1, delta);
            maxDistanceRenderer.transform.position = transform.position + (Vector3)delta.normalized * dashController.maxDistance;
            maxDistanceRenderer.transform.up = delta;

            cooldownDial.color = chargingColor;
            cooldownDial.fillAmount = dashController.chargeTimeRemaining / dashController.chargeTime;
            cooldownDial.enabled = false;
        } else if(dashController.currentState == DashAttackController.State.COOLDOWN) {
            cooldownDial.color = cooldownColor;
            cooldownDial.fillAmount = dashController.cooldownTimeRemaining / dashController.cooldownTime;
            cooldownDial.enabled = true;
        } else {
            cooldownDial.enabled = false;
        }
    }
}
