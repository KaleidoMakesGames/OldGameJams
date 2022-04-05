using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KMGPathfinding {
    [ExecuteAlways]
    [RequireComponent(typeof(CompositeCollider2D))]
    public class TraversibleSurface : MonoBehaviour {
        public Mesh mesh { get; private set; }

        private CompositeCollider2D compositeCollider;
        private Rigidbody2D rb2D;
        private void Update() {
            if(compositeCollider == null) {
                compositeCollider = GetComponent<CompositeCollider2D>();
            }
            if(compositeCollider == null) {
                compositeCollider = gameObject.AddComponent<CompositeCollider2D>();
            }
            if(rb2D == null) {
                rb2D = GetComponent<Rigidbody2D>();
            }
            rb2D.bodyType = RigidbodyType2D.Static;
            compositeCollider.isTrigger = true;
            compositeCollider.geometryType = CompositeCollider2D.GeometryType.Polygons;
        }
        [EasyButtons.Button()]
        public void Remesh() {
            if(mesh != null) {
                if(Application.isPlaying) {
                    Destroy(mesh);
                } else {
                    DestroyImmediate(mesh);
                }
            }
            mesh = GetComponent<CompositeCollider2D>().CreateMesh(true, true);
        }

        private void OnDrawGizmosSelected() {
            if(mesh != null) {
                for(int triangleNumber = 0; triangleNumber < mesh.triangles.Length/3; triangleNumber++) {
                    Vector3 a = mesh.vertices[mesh.triangles[triangleNumber * 3 + 0]];
                    Vector3 b = mesh.vertices[mesh.triangles[triangleNumber * 3 + 1]];
                    Vector3 c = mesh.vertices[mesh.triangles[triangleNumber * 3 + 2]];
                    Gizmos.DrawLine(a, b);
                    Gizmos.DrawLine(a, c);
                    Gizmos.DrawLine(b, c);
                }
            }
        }
    }
}