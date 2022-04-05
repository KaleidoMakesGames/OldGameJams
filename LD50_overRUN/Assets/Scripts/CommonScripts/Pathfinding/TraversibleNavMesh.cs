//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//namespace KMGPathfinding {
//    public class TraversibleNavMesh : PathfindingSystem {
//        public Vector2 size;
//        public Vector2 center;

//        public class MeshNode {
//            public Vector2 a, b, c;
//        }

//        public List<MeshNode> nodes;

//        private Graph<MeshNode> _graph;
//        private float Distance(MeshNode a, MeshNode b) {
////TODO
//            return 0;
//        }

//        public override List<Vector2> GetPath(Vector2 a, Vector2 b, float agentRadius) {
//            var path = _graph.AStarPath(GetNode(a), GetNode(b), Distance, Distance);
//            if (path == null) {
//                return null;
//            }
//            var waypoints = new List<Vector2>();
//            waypoints.Add(a);
//            waypoints.Add(b);

//            for (int i = 0; i < waypoints.Count; i++) {
//                int redundantWaypoints = 0;
//                for (int j = waypoints.Count - 1; j > i; j--) {
//                    var posA = waypoints[i];
//                    var posB = waypoints[j];
//                    if (!IsBlocked(posA, posB, agentRadius)) {
//                        redundantWaypoints = j - i - 1;
//                        break;
//                    }
//                }
//                if (redundantWaypoints > 0) {
//                    waypoints.RemoveRange(i + 1, redundantWaypoints);
//                }
//            }
//            return waypoints;
//        }
//        private bool IsBlocked(Vector2 a, Vector2 b, float agentRadius) {
//            foreach (var hit in Physics2D.CircleCastAll(a, agentRadius, b - a, (b - a).magnitude)) {
//                if (!hit.collider.isTrigger) {
//                    return true;
//                }
//            }
//            return false;
//        }

//        private bool IsBlocked(Vector2 position, float agentRadius) {
//            foreach (var hit in Physics2D.OverlapCircleAll(position, agentRadius)) {
//                if (!hit.isTrigger) {
//                    return true;
//                }
//            }
//            return false;
//        }

//        public MeshNode GetNode(Vector2 position) {
//            float closestDistance = Mathf.Infinity;
//            MeshNode closestNode = null;
//            foreach(var n in nodes) {
//                float distance = Vector2.Distance(position, GeometryUtils2D.ClosestPointInTriangle(position, n.a, n.b, n.c));
//                if(distance < closestDistance) {
//                    closestNode = n;
//                    closestDistance = distance;
//                }
//            }
//            return closestNode;
//        }

//        [EasyButtons.Button()]
//        public void Rebuild() {
//            var pathfindingColliders = (List<TraversibleSurface>)Physics2D.OverlapBoxAll(transform.TransformPoint(size), size, 0.0f).Select(x => x.GetComponentInParent<TraversibleSurface>()).Where(x => x != null);
//            _graph = new Graph<MeshNode>();
//            nodes.Clear();
//            foreach(var p in pathfindingColliders) {
//                MeshNode newNode = new MeshNode();
//            }
//        }

//        private void OnDrawGizmosSelected() {
//            Gizmos.DrawWireCube(transform.TransformPoint(center), size);
//        }
//    }
//}