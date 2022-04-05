using System.Collections;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace KMGPathfinding {
    [RequireComponent(typeof(Rigidbody2D))]
    public class AgentController : MonoBehaviour {
        public PathfindingSystem pathfindingSystem;
        [SerializeField]private Vector3 _destination;
        public Vector3 destination {
            get {
                return _destination;
            }
            set {
                _destination = value;
                UpdateWaypoints();
            }
        }

        public float movementSpeed;

        public List<Vector2> waypoints { get; private set; }

        private float distanceAlongWaypoints;
        public bool isMoving {
            get {
                return waypoints != null;
            }
        }
        private Vector2 lastDestination;

        private Rigidbody2D rb;

        private void Awake() {
            rb = GetComponent<Rigidbody2D>();
            if(pathfindingSystem == null) {
                pathfindingSystem = FindObjectOfType<PathfindingSystem>();
            }
        }

        public void Stop() {
            waypoints = null;
        }

        // Update is called once per frame
        void Update() {
            if (lastDestination != (Vector2)destination) {
                UpdateWaypoints();
            }
            if (waypoints != null && (waypoints.Count == 0 || (Vector2)transform.position == waypoints.Last())) {
                waypoints = null;
            }
        }
        private void FixedUpdate() {
            if(waypoints != null) {
                distanceAlongWaypoints += movementSpeed * Time.fixedDeltaTime;
                rb.MovePosition(GeometryUtils2D.PointAtDistanceAlongPath(waypoints, distanceAlongWaypoints, false));
            }
        }

        public void UpdateWaypoints() {
            waypoints = pathfindingSystem.GetPath(transform.position, destination);
            if (waypoints != null) {
                GeometryUtils2D.SnapToPath(waypoints, rb.position, out distanceAlongWaypoints);
            }
            lastDestination = destination;
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.yellow;
            if (waypoints != null) {
                for (int i = 0; i < waypoints.Count - 1; i++) {
                    Gizmos.DrawLine(waypoints[i], waypoints[i + 1]);
                }
            }
        }
    }
}