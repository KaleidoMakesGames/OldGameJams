using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace KMGPathfinding {
    public class TraversibleGrid : PathfindingSystem {
        public bool useIslands;

        public class GridNode {
            public Vector2 position;
            public int islandNumber;
        }

        private Graph<GridNode> _graph;

        private float Distance(GridNode a, GridNode b) {
            return Vector2.Distance(a.position, b.position);
        }

        public List<GridNode> nodes;

        [SerializeField] private Vector2 _size = Vector2.one;
        public Vector2 size {
            get {
                return _size * transform.localScale;
            }
        }
        public float spacing = 0.5f;
        public float agentRadius = 0.5f;

        private void Start() {
            BuildNodes();
        }

        public GridNode GetNode(Vector2 position) {
            return nodes.OrderBy(x => Vector2.Distance(position, x.position)).FirstOrDefault();
        }

        public GridNode GetNode(Vector2 position, int island) {
            return nodes.Where(x => x.islandNumber == island).OrderBy(x => Vector2.Distance(position, x.position)).FirstOrDefault();
        }

        public override List<Vector2> GetPath(Vector2 a, Vector2 b) {
            var startNode = GetNode(a);
            var endNode = useIslands ? GetNode(b, startNode.islandNumber) : GetNode(b);
            var path = _graph.AStarPath(startNode, endNode, Distance, Distance);
            if(path == null) {
                return null;
            }
            var waypoints = new List<Vector2>();
            waypoints.Add(a);
            waypoints.AddRange(path.Select(x => x.position));
            if (!IsBlocked(waypoints.Last(), b)) {
                waypoints.Add(b);
            }

            for(int i = 0; i < waypoints.Count; i++) {
                int redundantWaypoints = 0;
                for (int j = waypoints.Count-1; j > i; j--) {
                    var posA = waypoints[i];
                    var posB = waypoints[j];
                    if (!IsBlocked(posA, posB)) {
                        redundantWaypoints = j - i - 1;
                        break;
                    }
                }
                if (redundantWaypoints > 0) {
                    waypoints.RemoveRange(i + 1, redundantWaypoints);
                }
            }
            return waypoints;
        }

        private void BuildNodes() {
            nodes = new List<GridNode>();
            _graph = new Graph<GridNode>();
            for (float x = -size.x; x <= size.x; x += spacing) {
                for (float y = -size.y; y <= size.y; y += spacing) {
                    GridNode newNode = new GridNode();
                    newNode.position = new Vector3(x, y) + transform.position;
                    newNode.islandNumber = 0;
                    if (IsBlocked(newNode.position)) {
                        continue;
                    }
                    nodes.Add(newNode);
                }
            }

            foreach(GridNode a in nodes) {
                foreach(GridNode b in nodes) {
                    if(a != b && Vector2.Distance(a.position, b.position) <= 1.5*spacing) {
                        if (!IsBlocked(a.position, b.position)) {
                            _graph.AddNeighbor(a, b);
                        }
                    }
                }
            }

            if (useIslands) {
                var nodesWithoutIsland = nodes;
                int islandNumber = 1;
                while (nodesWithoutIsland.Count > 0) {
                    AssignIslandNumber(nodesWithoutIsland[0], islandNumber);
                    nodesWithoutIsland = nodesWithoutIsland.Where(x => x.islandNumber == 0).ToList();
                    islandNumber += 1;
                }
            }
        }

        private void AssignIslandNumber(GridNode node, int number) {
            var toExpand = new Queue<GridNode>();
            node.islandNumber = number;
            toExpand.Enqueue(node);
            while(toExpand.Count > 0) {
                var n = toExpand.Dequeue();
                foreach(var neighbor in _graph.Neighbors(n)) {
                    if(neighbor.islandNumber != number) {
                        neighbor.islandNumber = number;
                        toExpand.Enqueue(neighbor);
                    } 
                }
            }
        }

        private bool IsBlocked(Vector2 a, Vector2 b) {
            foreach (var hit in Physics2D.CircleCastAll(a, agentRadius, b-a, (b-a).magnitude)) {
                if (!hit.collider.isTrigger && hit.collider.GetComponentInParent<Rigidbody2D>() == null) {
                    return true;
                }
            }
            return false;
        }

        private bool IsBlocked(Vector2 position) {
            foreach(var hit in Physics2D.OverlapCircleAll(position, agentRadius)) {
                if(!hit.isTrigger && hit.GetComponentInParent<Rigidbody2D>() == null) {
                    return true;
                }
            }
            return false;
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = new Color(1.0f, 1.0f, 1.0f, 0.2f);
            Gizmos.DrawWireCube(transform.position, size * 2);
            for (float x = -size.x; x <= size.x; x += spacing) {
                Gizmos.DrawLine(new Vector2(x + transform.position.x, transform.position.y - size.y),
                    new Vector2(x + transform.position.x, transform.position.y + size.y));
            }
            for (float y = -size.y; y <= size.y; y += spacing) {
                Gizmos.DrawLine(new Vector2(transform.position.x - size.x, transform.position.y + y),
                    new Vector2(transform.position.x + size.x, transform.position.y + y));
            }
            if (nodes != null) {
                foreach(var node in nodes) {
                    if (useIslands) {
                    }
                    Gizmos.DrawWireSphere(node.position, spacing / 8.0f);
                }
            }
            Gizmos.color = Color.green;
            if(_graph != null) {
                foreach(var node in nodes) {
                    foreach (var neighbor in _graph.Neighbors(node)) {
                        Gizmos.DrawLine(node.position, (node.position + neighbor.position)/2);
                    }
                }
            }
        }

        private void OnValidate() {
            _size = new Vector2(Mathf.Max(0, _size.x), Mathf.Max(0, _size.y));
            spacing = Mathf.Max(spacing, 0.001f);
        }
    }
}