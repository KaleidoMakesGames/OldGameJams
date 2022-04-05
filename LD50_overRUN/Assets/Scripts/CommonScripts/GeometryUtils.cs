using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GeometryUtils2D {
    public static Vector2 ClosestPointInTriangle(Vector2 point, Vector2 a, Vector2 b, Vector2 c) {
        if (IsPointInTriangle(point, a, b, c)) {
            return point;
        }
        Vector2 closestA = ClosestOnSegment(point, a, b);
        Vector2 closestB = ClosestOnSegment(point, a, c);
        Vector2 closestC = ClosestOnSegment(point, b, c);

        float distanceA, distanceB, distanceC;

        distanceA = Vector2.Distance(closestA, point);
        distanceB = Vector2.Distance(closestB, point);
        distanceC = Vector2.Distance(closestC, point);

        if(distanceA < distanceB && distanceA < distanceC) {
            return closestA;
        }
        if (distanceB < distanceA && distanceB < distanceC) {
            return closestB;
        }
        return closestC;
    }

    public static Vector2 Project(Vector2 a, Vector2 on) {
        return Vector2.Dot(a, on.normalized) * on.normalized;
    }

    public static Vector2 ClosestOnSegment(Vector2 point, Vector2 a, Vector2 b) {
        Vector2 segment = b - a;
        Vector2 pointVector = point - a;
        float dot = Vector2.Dot(pointVector, segment.normalized);
        if(dot < 0) {
            return a;
        }
        if(dot > segment.magnitude) {
            return b;
        }
        return dot * segment.normalized + a;
    }

    private static float sign(Vector2 p1, Vector2 p2, Vector2 p3) {
        return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
    }

    public static bool IsPointInTriangle(Vector2 point, Vector2 a, Vector2 b, Vector2 c) {
        float d1, d2, d3;
        bool has_neg, has_pos;

        d1 = sign(point, a, b);
        d2 = sign(point, b, c);
        d3 = sign(point, c, a);

        has_neg = (d1 < 0) || (d2 < 0) || (d3 < 0);
        has_pos = (d1 > 0) || (d2 > 0) || (d3 > 0);

        return !(has_neg && has_pos);
    }
    public static List<Vector2> ParameterizeSpline(UnityEngine.U2D.Spline spline, float resolution, bool closed) {
        resolution = Mathf.Abs(resolution);
        if(resolution == Mathf.Infinity) {
            return null;
        }
        List<Vector2> path = new List<Vector2>();
        for (int i = 0; i < spline.GetPointCount() - (closed ? 0 : 1); i++) {
            for (float t = 0.0f; t < 1; t += 1.0f/resolution) {
                path.Add(SampleAlongSplineSegment(spline, i, t));
            }
        }
        return path;
    }
    public static Vector2 PointAtDistanceAlongPath(List<Vector2> points, float distanceAlong, bool closed) {
        return PointAtDistanceAlongPath(points, PathLength(points, closed), distanceAlong, closed);
    }

    public static Vector2 PointAtDistanceAlongPath(List<Vector2> points, float cachedTotalDistance, float distanceAlong, bool closed) {
        distanceAlong = closed ? distanceAlong % cachedTotalDistance : Mathf.Clamp(distanceAlong, 0.0f, cachedTotalDistance);
        for(int i = 0; i < points.Count; i++) {
            Vector2 delta = points[(int)Mathf.Repeat(i+1, points.Count)] - points[i];
            if(delta.magnitude >= distanceAlong) {
                return points[i] + delta.normalized * distanceAlong;
            } else {
                distanceAlong -= delta.magnitude;
            }
        }
        return points[points.Count - 1];
    }

    public static float PathLength(List<Vector2> points, bool closed) {
        float distance = 0.0f;
        for (int i = 0; i < points.Count - (closed ? 0 : 1); i++) {
            distance += Vector2.Distance(points[i], points[(int)Mathf.Repeat(i + 1, points.Count)]);
        }
        return distance;
    }

    // Returns the point but snapped to the path, as well as the distance that this point is along the path.
    public static Vector2 SnapToPath(List<Vector2> path, Vector2 point, out float distanceAlongPath) {
        // Find the segment that we are currently on:
        int closestSegmentIndex = -1;
        float distanceToClosestSegment = Mathf.Infinity;
        Vector2 closestSegmentPoint = path[0];
        for (int i = 0; i < path.Count - 1; i++) {
            Vector2 a = path[i];
            Vector2 b = path[i + 1];
            Vector2 onSegment = ClosestOnSegment(point, a, b);
            float distance = Vector2.Distance(point, onSegment);
            if(distance < distanceToClosestSegment) {
                distanceToClosestSegment = distance;
                closestSegmentIndex = i;
                closestSegmentPoint = onSegment;
            }
        }

        // How far along the path is the closest point?
        distanceAlongPath = 0;
        for (int i = 0; i < closestSegmentIndex; i++) {
            distanceAlongPath += Vector2.Distance(path[i], path[i + 1]);
        }

        return closestSegmentPoint;
    }

    public static Vector2 MoveAlongPath(List<Vector2> path, Vector2 currentPosition, float delta, bool closed) {
        float distanceAlong;
        SnapToPath(path, currentPosition, out distanceAlong);
        distanceAlong += delta;
        return PointAtDistanceAlongPath(path, distanceAlong, closed);
    }

    public static Vector3 SampleAlongSplineSegment(UnityEngine.U2D.Spline spline, int segmentNumber, float t) {
        int b = (int)Mathf.Repeat(segmentNumber + 1, spline.GetPointCount());
        Vector3 startPoint = spline.GetPosition(segmentNumber);
        Vector3 startTangent = startPoint + spline.GetRightTangent(segmentNumber);
        Vector3 endPoint = spline.GetPosition(b);
        Vector3 endTangent = endPoint + spline.GetLeftTangent(b);
        return UnityEngine.U2D.BezierUtility.BezierPoint(startPoint, startTangent, endTangent, endPoint, t);
    }
}