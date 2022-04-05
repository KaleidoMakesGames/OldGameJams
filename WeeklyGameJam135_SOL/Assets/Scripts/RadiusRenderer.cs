using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusRenderer : MonoBehaviour
{
    public LineRenderer lineRenderer;

    private static CircleCollider2D cometCollider;

    private void Start() {
        if (cometCollider == null) {
            cometCollider = FindObjectOfType<CometController>().GetComponent<CircleCollider2D>(); ;
        }

        Redraw();
    }

    private void Redraw() {
        float selfScale = cometCollider.radius + transform.parent.localScale.x/2.0f;

        Transform p = transform.parent;
        transform.SetParent(null, true);
        transform.localScale = new Vector3(1.0f, 0.0f, 1.0f) * selfScale * 2;
        transform.SetParent(p);

        List<Vector3> positions = new List<Vector3>();
        for (float angle = 0.0f; angle <= 360.0f; angle += 5.0f) {
            positions.Add(new Vector3(Mathf.Cos(angle*Mathf.Deg2Rad), 0.0f, Mathf.Sin(angle*Mathf.Deg2Rad))*0.5f);
        }
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }
}
