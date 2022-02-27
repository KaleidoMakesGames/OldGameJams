using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace KMGPathfinding {
    public class SimpleGridPathfinder : MonoBehaviour {
        public PathfindingSystem<SimpleGridNode, SimpleGridPathfinderAgent> pathfindingSystem;
        public Vector2 gridSize;
        public Vector2 gridSpacing;
    }

    public class SimpleGridNode : IPathfindingNode<SimpleGridNode> {
        public Vector2 position;
        private SimpleGridPathfinder _pathfinder;
        public SimpleGridNode(SimpleGridPathfinder gridPathfinder) {
            _pathfinder = gridPathfinder;
        }

        public Vector2 ApproxPosition() {
            return position;
        }

        public bool IsConnected(SimpleGridNode otherNode) {
            var gridNode = otherNode;
            var delta = position - gridNode.position;
            return Mathf.Abs(delta.x) > _pathfinder.gridSpacing.x || Mathf.Abs(delta.y) > _pathfinder.gridSpacing.y;
        }
    }
}