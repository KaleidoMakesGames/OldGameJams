using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PathCollider : MonoBehaviour
{
    public PathController pathController;
    public PolygonCollider2D polygonCollider;
    public EdgeCollider2D edgeCollider;

    // Start is called before the first frame update
    void Start()
    {
        polygonCollider.points = pathController.path.ToArray();
        edgeCollider.points = pathController.path.ToArray();
    }
}
