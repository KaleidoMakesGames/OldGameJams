using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Navigation {
    public class Node {
        public List<Node> connectedNodes;
        public Vector2 position;

        public Node(Vector2 position) {
            connectedNodes = new List<Node>();
            this.position = position;
        }
    }

    [RequireComponent(typeof(Collider2D))]
    public class NavigatableAreaGenerator : MonoBehaviour {
        public Bounds bounds { 
            get {
                Collider2D collider = GetComponent<Collider2D>();
                if (GetComponent<CompositeCollider2D>() != null) {
                    collider = GetComponent<CompositeCollider2D>();
                }
                return collider.bounds;
            }
        }

        private Dictionary<Vector3, List<Node>> boundsCache = new Dictionary<Vector3, List<Node>>();
        private float lastSpacing;
        private Vector2 lastOffset;

        public NavigationAreaSettings settings;

        public List<Node> GetTraversibleNodes(Bounds traversingBounds) {
            if(lastSpacing != settings.nodeSpacing || lastOffset != settings.offset) {
                boundsCache.Clear();
            }
            lastSpacing = settings.nodeSpacing;
            lastOffset = settings.offset;
            if(boundsCache.ContainsKey(traversingBounds.size) && boundsCache[traversingBounds.size] != null) {
                return boundsCache[traversingBounds.size];
            }

            float nodeSpacing = settings.nodeSpacing;

            if (nodeSpacing == 0.0f) {
                return null;
            }

            Dictionary<Vector2, Node> nodesMap = new Dictionary<Vector2, Node>();

            Bounds worldBounds = bounds;
            worldBounds.min += (Vector3)settings.offset;

            bool offset = false;
            for (float y = worldBounds.min.y; y <= worldBounds.max.y; y += nodeSpacing / 2.0f) {
                offset = !offset;
                for (float x = worldBounds.min.x + (offset ? nodeSpacing / 2.0f : 0.0f); x <= worldBounds.max.x; x += nodeSpacing) {
                    Vector2 position = new Vector2(x, y);
                    nodesMap.Add(position, new Node(position));
                }
            }

            foreach (Node node in nodesMap.Values) {
                List<Vector2> neighbors = new List<Vector2>();
                neighbors.Add(node.position + Vector2.up * nodeSpacing);
                neighbors.Add(node.position + Vector2.down * nodeSpacing);
                neighbors.Add(node.position + Vector2.left * nodeSpacing);
                neighbors.Add(node.position + Vector2.right * nodeSpacing);

                neighbors.Add(node.position + Vector2.up * nodeSpacing / 2 + Vector2.right * nodeSpacing / 2);
                neighbors.Add(node.position + Vector2.down * nodeSpacing / 2 + Vector2.right * nodeSpacing / 2);
                neighbors.Add(node.position + Vector2.up * nodeSpacing / 2 + Vector2.left * nodeSpacing / 2);
                neighbors.Add(node.position + Vector2.down * nodeSpacing / 2 + Vector2.left * nodeSpacing / 2);

                foreach (Vector2 neighbor in neighbors) {
                    if (nodesMap.ContainsKey(neighbor)) {
                        node.connectedNodes.Add(nodesMap[neighbor]);
                    }
                }
            }

            foreach (Node node in nodesMap.Values) {
                for (int nodeIndex = node.connectedNodes.Count - 1; nodeIndex >= 0; nodeIndex--) {
                    Vector2 start = node.position;
                    Vector2 end = node.connectedNodes[nodeIndex].position;
                    Vector2 castVector = end - start;
                    foreach(RaycastHit2D hit in Physics2D.BoxCastAll(start, traversingBounds.size, 0.0f, castVector.normalized, castVector.magnitude)) { 
                        if (!hit.collider.isTrigger && hit.collider.GetComponent<NavigatableAreaBlocker>() != null) {
                            node.connectedNodes.RemoveAt(nodeIndex);
                        }
                    }

                }
            }

            List<Node> nodes = new List<Node>(nodesMap.Values);

            for(int i = nodes.Count-1; i >= 0; i--) {
                if(nodes[i].connectedNodes.Count == 0) {
                    nodes.RemoveAt(i);
                }
            }

            boundsCache.Add(traversingBounds.size, nodes);
            return nodes;
        }
    }
}
