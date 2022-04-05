using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigator
{
    public delegate void PathReadyFunc(List<Vector2Int> path);

    public static void ComputePath(MovementController movingElement, Vector2Int goal, PathReadyFunc onComplete) {
        movingElement.StartCoroutine(ComputePathAsync(movingElement, goal, onComplete));
    }


    private static IEnumerator ComputePathAsync(MovementController movingElement, Vector2Int goal, PathReadyFunc onComplete) {
        Vector2Int start = movingElement.gridElement.position;

        Vector2Int? closestGoal = FindClosestToGoal(goal);
        if(closestGoal.HasValue) {
            goal = closestGoal.Value;
        } else {
            onComplete.Invoke(null);
            yield break;
        }

        List<Vector2Int> openSet = new List<Vector2Int> {
            start
        };

        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();

        Dictionary<Vector2Int, float> gScore = new Dictionary<Vector2Int, float> {
            [start] = 0
        };

        Dictionary<Vector2Int, float> fScore = new Dictionary<Vector2Int, float> {
            [start] = H(start, goal)
        };

        float lastFrameTime = Time.realtimeSinceStartup;
        while(openSet.Count > 0) {
            
            Vector2Int current = openSet[0];
            foreach (Vector2Int i in openSet) {
                if(fScore[i] < fScore[current]) {
                    current = i;
                }
            }

            if(current == goal) {
                onComplete.Invoke(ReconstructPath(cameFrom, current));
                yield break;
            }

            openSet.Remove(current);

            foreach(Vector2Int neighbor in AvailableNeighbors(movingElement, current)) {
                if(!gScore.ContainsKey(neighbor)) {
                    gScore[neighbor] = Mathf.Infinity;
                }
                float tentativeScore = gScore[current] + Vector2Int.Distance(current, neighbor);
                if(tentativeScore < gScore[neighbor]) {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeScore;
                    fScore[neighbor] = gScore[neighbor] + H(neighbor, goal);
                    if(!openSet.Contains(neighbor)) {
                        openSet.Add(neighbor);
                    }
                }
            }

            float frameRate = Application.targetFrameRate == -1 ? 60.0f : Application.targetFrameRate;
            if(Time.realtimeSinceStartup - lastFrameTime >= 1/frameRate) {
                lastFrameTime = Time.realtimeSinceStartup;
                yield return null;
            }
        }

        onComplete.Invoke(null);
    }

    private static Vector2Int? FindClosestToGoal(Vector2Int goal) {
        for(int radius = 0; radius < 5; radius++) {
            for(int x = -radius; x <= radius; x++) {
                for(int y = -radius; y <= radius; y++) {
                    if(x == -radius || x == radius || y == -radius || y == radius) {
                        Vector2Int pointToCheck = new Vector2Int(x, y) + goal;
                        if(IsReachable(pointToCheck)) {
                            return pointToCheck;
                        }
                    }
                }
            }
        }
        return null;
    }

    private static List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current) {
        List<Vector2Int> path = new List<Vector2Int> {
            current
        };
        while (cameFrom.ContainsKey(current)) {
            current = cameFrom[current];
            path.Insert(0, current);
        }
        return path;
    }


    private static List<Vector2Int> AvailableNeighbors(MovementController movingElement, Vector2Int point) {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        for(int x = -1; x <= 1; x++) {
            for(int y = -1; y <= 1; y++) {
                if (x == 0 && y == 0) {
                    continue;
                }
                Vector2Int drive = new Vector2Int(x, y);
                if (movingElement.CanDoMove(point, drive)) {
                    neighbors.Add(point + drive);
                }
            }
        }
        return neighbors;
    }

    private static bool IsReachable(Vector2Int point) {
        return WorldGrid.Instance.CanOccupy(point);
    }

    private static float H(Vector2Int point, Vector2Int goal) {
        return Vector2Int.Distance(point, goal);
    }
}
