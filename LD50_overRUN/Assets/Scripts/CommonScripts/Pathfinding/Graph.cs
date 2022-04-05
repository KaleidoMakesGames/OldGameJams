using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KMGPathfinding {
    public class Graph<Node> where Node : class {
        public Dictionary<Node, List<Node>> _neighbors;

        public delegate float BinaryFunc(Node a, Node b);

        public Graph() {
            _neighbors = new Dictionary<Node, List<Node>>();
        }

        public void AddNeighbor(Node node, Node neighbor) {
            if(!_neighbors.ContainsKey(node)) {
                _neighbors[node] = new List<Node>();
            }
            _neighbors[node].Add(neighbor);
        }

        public List<Node> Neighbors(Node node) {
            if(!_neighbors.ContainsKey(node)) {
                _neighbors[node] = new List<Node>();
            }
            return _neighbors[node];
        }

        public void RemoveNeighbor(Node node, Node neighbor) {
            if(_neighbors.ContainsKey(node)) {
                _neighbors[node].Remove(neighbor);
            }
        }

        public List<Node> AStarPath(Node start, Node goal, BinaryFunc costFunc, BinaryFunc heuristicFunc) {
            List<Node> openSet = new List<Node>();
            openSet.Add(start);

            Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();

            Dictionary<Node, float> gScore = new Dictionary<Node, float>();
            gScore[start] = 0;

            Dictionary<Node, float> fScore = new Dictionary<Node, float>();
            fScore[start] = heuristicFunc(start, goal);

            while (openSet.Count > 0) {
                Node current = null;
                float minScore = Mathf.Infinity;
                foreach(var n in openSet) {
                    if(fScore[n] < minScore) {
                        current = n;
                        minScore = fScore[n];
                    }
                }

                if(current == goal) {
                    List<Node> path = new List<Node>();
                    path.Add(current);
                    while(cameFrom.ContainsKey(current)) {
                        current = cameFrom[current];
                        path.Insert(0, current);
                    }
                    return path;
                }

                openSet.Remove(current);
                foreach(var neighbor in _neighbors[current]) {
                    if (!gScore.ContainsKey(neighbor)) {
                        gScore[neighbor] = Mathf.Infinity;
                    }
                    if(!fScore.ContainsKey(neighbor)) {
                        fScore[neighbor] = Mathf.Infinity;
                    }
                    float tgScore = gScore[current] + costFunc(current, neighbor);
                    if(tgScore < gScore[neighbor]) {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tgScore;
                        fScore[neighbor] = tgScore + heuristicFunc(neighbor, goal);
                        if (!openSet.Contains(neighbor)) {
                            openSet.Add(neighbor);
                        }
                    }
                }
            }

            return null;
        }
    }
}