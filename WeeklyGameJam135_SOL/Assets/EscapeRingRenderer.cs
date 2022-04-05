using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EscapeRingRenderer : MonoBehaviour
{
    public LineRenderer r;
    public float heightToEscape;

    public UnityEvent OnWinEvent;

    public CometController c;
    // Update is called once per frame

    private bool hasWon;

    private void Awake() {
        hasWon = false;
    }

    private void Update() {
        r.enabled = c.currentState != CometController.State.Idle;

        float current = Vector2.Distance(c.transform.position, c.massiveBody.transform.position);
        if(current >= heightToEscape) {
            OnWin();
        }
    }

    void OnWin() {
        c.currentState = CometController.State.FreeAtLast;
        c.GetComponent<Rigidbody2D>().velocity = (c.cometCam.transform.position.normalized - c.massiveBody.transform.position).normalized * c.launchVelocity;

        if (!hasWon) {
            OnWinEvent.Invoke();
            hasWon = true;
        }
    }

    void OnValidate()
    {
        List<Vector3> positions = new List<Vector3>();
        for (float angle = 0.0f; angle <= 360.0f; angle += 1.0f) {
            positions.Add(new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0.0f, Mathf.Sin(angle * Mathf.Deg2Rad)) * 0.5f);
        }
        r.positionCount = positions.Count;
        r.SetPositions(positions.ToArray());
        transform.localScale = Vector3.one * (heightToEscape + r.widthMultiplier / 2) * 2;
    }
}
