using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using System.IO;

public class PathDrawer : MonoBehaviour {
    public PathController drawnPathController;
    public PathController prefabPath;

    [System.Serializable]
    public class PathEvent : UnityEvent<List<Vector2>> { }
    public PathEvent OnClose;

    public List<Vector2> points { get; set; } 
    public float controlPointResolution;

    public float maxLength;

    private void Awake() {
        points = new List<Vector2>();
    }

    public void SetCurrentPoint(Vector2 point) {
        drawnPathController.GetComponent<PathRenderer>().color = prefabPath.GetComponent<PathRenderer>().color;

        if (points.Count == 0) {
            points.Add(point);
            points.Add(point);
        }
        points[points.Count - 1] = point;

        drawnPathController.path = points;

        List<Vector2> loop = CalculateClosedLoop(points, points.Count-1);
        if (loop != null) {
            ClosePath(loop);
        } else {
            float dist = Vector2.Distance(points[points.Count - 2], points[points.Count - 1]);
            if (dist >= 1.0f / controlPointResolution) {
                points.Add(point);

                if(PathLength() > maxLength) {
                    points.RemoveAt(0);
                }
            }
        }
    }

    public void SpawnPath(List<Vector2> path) {
        var newWall = Instantiate(prefabPath.gameObject).GetComponent<PathController>();
        newWall.transform.position = path[0];
        newWall.path = path.ConvertAll(v => (Vector2)newWall.transform.InverseTransformPoint(v));
    }

    private float PathLength() {
        float length = 0.0f;
        for(int i = 1; i < points.Count; i++) {
            length += Vector2.Distance(points[i], points[i - 1]);
        }
        return length;
    }

    static float Cross(Vector2 a, Vector2 b) {
        return a.x * b.y - b.x * a.y;
    }
    // The main function that returns true if line segment 'p1q1' 
    // and 'p2q2' intersect. 
    static Vector2? FindIntersection(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2) {
        Vector2 p = a1;
        Vector2 r = a2 - p;
        Vector2 q = b1;
        Vector2 s = b2 - q;

        float rxs = Cross(r, s);
        if (rxs == 0) {
            return null;
        }

        float t = Cross(q - p, s) / rxs;
        float u = Cross(q - p, r) / rxs;
        if (0 <= t && t <= 1 && 0 <= u && u <= 1) {
            return p + t * r;
        } else {
            return null;
        }
    }

    public List<Vector2> CalculateClosedLoop(List<Vector2> points, int changingStartIndex) {
        for (int i = changingStartIndex; i < points.Count; i++) {
            if(i-1 < 0) {
                continue;
            }

            Vector2 changingSegmentStart = points[i-1];
            Vector2 changingSegmentEnd = points[i];
            for (int j = 1; j < points.Count; j++) {
                Vector2 segmentStart = points[j - 1];
                Vector2 segmentEnd = points[j];

                if(j == i || j - 1 == i || j == i-1) {
                    continue;
                }

                var intersection = FindIntersection(changingSegmentStart, changingSegmentEnd,
                                                    segmentStart, segmentEnd);

                if (intersection.HasValue) {
                    int endIndex = Mathf.Max(i, j);
                    int startIndex = Mathf.Min(i, j);
                    var subset = points.GetRange(startIndex, endIndex-startIndex+1);
                    
                    return subset;
                }
            }
        }
        return null;
    }

    public void ClosePath(List<Vector2> path) {
        SpawnPath(path);
        OnClose.Invoke(path);
        ClearPath();
    }

    public void ClearPath() {
        points.Clear();
        drawnPathController.path.Clear();
    }

    private Vector2 CatmullRomInterpolateP1P2(Vector2 P0, Vector2 P1, Vector2 P2, Vector2 P3, float t) {
        return (0.5f * ((2 * P1) + (-P0 + P2) * t + (2 * P0 - 5 * P1 + 4 * P2 - P3) * t * t + (-P0 + 3 * P1 - 3 * P2 + P3) * t * t * t));
    }
}
