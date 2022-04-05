using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Navigation {
    public class NavigatableAreaPathfinder : MonoBehaviour {
        public Collider2D selfCollider;

        private NavigatableAreaGenerator area {
            get {
                Bounds myBounds = selfCollider.bounds;
                foreach(NavigatableAreaGenerator generator in FindObjectsOfType<NavigatableAreaGenerator>()) {
                    if(generator.bounds.Intersects(myBounds)) {
                        return generator;
                    }
                }
                return null;
            }
        }

        public float maxTimeWithoutUpdate;

        private List<Node> _nodes;
        private List<Node> nodes {
            get {
                if(!enabled) {
                    return null;
                }
                if (_nodes == null) {
                    RecomputeNodes();
                }
                return _nodes;
            }
            set {
                _nodes = value;
            }
        }
        
        public void RecomputeNodes() {
            if(area == null) {
                Debug.LogError(this + " is not on a NavigatableArea.");
                return;
            }
            nodes = area.GetTraversibleNodes(selfCollider.bounds);
        }

        public delegate void PathFinishedFunc(List<Vector2> path);

        private void Update() {
            
        }

        public IEnumerator FindPathToTarget(Vector2 target, PathFinishedFunc OnPathFinished) {
            List<Node> closedSet = new List<Node>();

            List<Node> openSet = new List<Node>();

            Node start = GetClosestNode(selfCollider.bounds.center);
            Node goal = GetClosestNode(target);

            openSet.Add(start);

            Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();

            Dictionary<Node, float> gScore = new Dictionary<Node, float>();
            Dictionary<Node, float> fScore = new Dictionary<Node, float>();
            foreach (Node node in nodes) {
                gScore.Add(node, Mathf.Infinity);
                fScore.Add(node, Mathf.Infinity);
            }

            gScore[start] = 0.0f;
            fScore[start] = GetHeuristic(start, goal);

            float lastUpdateTime = Time.realtimeSinceStartup;
            
            while(openSet.Count > 0) {
                if(Time.realtimeSinceStartup - lastUpdateTime > maxTimeWithoutUpdate) {
                    yield return null;
                    lastUpdateTime = Time.realtimeSinceStartup;
                }

                Node current = openSet[0];
                foreach(Node node in openSet) {
                    if(fScore[node] < fScore[current]) {
                        current = node;
                    }
                }

                if(current == goal) {
                    OnPathFinished(BuildPath(cameFrom, current));
                    yield break;
                }

                openSet.Remove(current);
                closedSet.Add(current);

                foreach (Node neighbor in current.connectedNodes) {
                    if (closedSet.Contains(neighbor)) {
                        continue;
                    }

                    float tentativeGScore = gScore[current] + Vector2.Distance(current.position, neighbor.position);

                    if (!openSet.Contains(neighbor)) {
                        openSet.Add(neighbor);
                    } else if (tentativeGScore >= gScore[neighbor]) {
                        continue;
                    }
                    
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + GetHeuristic(neighbor, goal);
                }
            }
            OnPathFinished(null);
        }

        private List<Vector2> BuildPath(Dictionary<Node, Node> cameFrom, Node current) {
            List<Vector2> newPath = new List<Vector2>();
            newPath.Add(current.position);
            while(cameFrom.ContainsKey(current)) {
                current = cameFrom[current];
                newPath.Add(current.position);
            }

            newPath.Reverse();

            int inflectionIndex = 0;
            Vector2 currentPosition = selfCollider.bounds.center;
            float? lastDistance = null;
            for(int i = 0; i < newPath.Count; i++) {
                float distance = Vector2.Distance(newPath[i], currentPosition);
                if(lastDistance.HasValue && distance > lastDistance) {
                    inflectionIndex = i;
                    break;
                }
                lastDistance = distance;
            }

            newPath.RemoveRange(0, inflectionIndex);

            return newPath;
        } 

        private float GetHeuristic(Node a, Node goal) {
            return Vector2.Distance(a.position, goal.position);
        }

        private Node GetClosestNode(Vector2 position) {
            float closestDistance = Mathf.Infinity;
            Node closestNode = null;
            foreach(Node node in nodes) {
                float distance = Vector2.Distance(position, node.position);
                if (distance < closestDistance) {
                    closestDistance = distance;
                    closestNode = node;
                }
            }

            return closestNode;
        }

        private void OnDrawGizmosSelected() {
            if (nodes != null) {
                foreach (Node node in nodes) {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(node.position, 0.1f);
                    foreach (Node adjacent in node.connectedNodes) {
                        Gizmos.DrawLine(node.position, adjacent.position);
                    }
                }
            }
        }
    }
}
