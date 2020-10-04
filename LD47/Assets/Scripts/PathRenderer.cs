using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathRenderer : MonoBehaviour
{
    public Color color;
    public LineRenderer lineRenderer;
    public PathController pathController;

    // Update is called once per frame
    void Update()
    {
        lineRenderer.material.color = color;
        lineRenderer.positionCount = pathController.path.Count;
        lineRenderer.SetPositions(pathController.path.ConvertAll<Vector3>(delegate(Vector2 a) { return a; }).ToArray());
    }
}
