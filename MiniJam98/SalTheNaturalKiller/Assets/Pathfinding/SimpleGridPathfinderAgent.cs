using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KMGPathfinding {
    public class SimpleGridPathfinderAgent : MonoBehaviour, IPathfinderAgent<SimpleGridNode> {
        public float radius;

        private static Dictionary<float, Dictionary<SimpleGridNode, bool>> _canOccupyCache;

        public SimpleGridPathfinderAgent() {
            if (_canOccupyCache == null) {
                _canOccupyCache = new Dictionary<float, Dictionary<SimpleGridNode, bool>>();
            }
        }

        public bool CanOccupyNode(SimpleGridNode node) {
            if (!_canOccupyCache.ContainsKey(radius)) {
                _canOccupyCache[radius] = new Dictionary<SimpleGridNode, bool>();
            }

            if (!_canOccupyCache[radius].ContainsKey(node)) {
                _canOccupyCache[radius][node] = true;
                foreach (var hit in Physics2D.OverlapCircleAll(node.position, radius)) {
                    if (!hit.isTrigger && hit.GetComponentInParent<SimpleGridPathfinderAgent>() != this) {
                        _canOccupyCache[radius][node] = false;
                        break;
                    }
                }
            }

            return _canOccupyCache[radius][node];
        }

        public float TraversalCost(SimpleGridNode from, SimpleGridNode to) {
            return 0;
        }
    }

}