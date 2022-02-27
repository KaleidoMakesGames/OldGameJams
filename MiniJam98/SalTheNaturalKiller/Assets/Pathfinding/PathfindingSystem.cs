using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace KMGPathfinding {
    public interface IPathfindingNode<NodeType> where NodeType : class, IPathfindingNode<NodeType>{
        public bool IsConnected(NodeType otherNode);
        public Vector2 ApproxPosition();
    }

    public interface IPathfinderAgent<NodeType> where NodeType : class, IPathfindingNode<NodeType> {
        public float TraversalCost(NodeType from, NodeType to);
        public bool CanOccupyNode(NodeType node);
    }

    public class PathfindingSystem<NodeType, AgentType> where NodeType : class, IPathfindingNode<NodeType> where AgentType : class, IPathfinderAgent<NodeType> {
        private Graph<NodeType> _graph;
        private List<NodeType> _nodes;

        private bool _isStale;

        public PathfindingSystem() {
            _graph = new Graph<NodeType>();
            _nodes = new List<NodeType>();
        }

        public void AddNode(NodeType node) {
            if(_nodes.Contains(node)) {
                return;
            }
            _nodes.Add(node);
            _isStale = true;
        }

        public void RemoveNode(NodeType node) {
            if(!_nodes.Contains(node)) {
                return;
            }
            _nodes.Remove(node);
            _isStale = true;
        }

        public void Rebuild() {
            _graph = new Graph<NodeType>();
            foreach(var a in _nodes) {
                foreach(var b in _nodes) {
                    if(a != b && a.IsConnected(b)) {
                        _graph.AddNeighbor(a, b);
                    }
                }
            }
        }
        public List<NodeType> GetPath(NodeType a, NodeType b, AgentType agent) {
            if(_isStale) {
                Rebuild();
            }
            return _graph.AStarPath(a, b, agent.TraversalCost, (x, y) => Vector2.Distance(x.ApproxPosition(), y.ApproxPosition()), agent.CanOccupyNode);
        }
    }
}