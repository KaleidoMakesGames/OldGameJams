using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace KMGMovement2D {
    [RequireComponent(typeof(Rigidbody2D))]
    [ExecuteAlways]
    public class MovingPlatformController : MonoBehaviour {
        public enum WrapMode { NONE, LOOP, PINGPONG }
        public WrapMode wrapMode;
        public float speed;
        public bool reverse;
        [Range(0, 1)] public float friction;
        public List<Waypoint> waypoints;

        [System.Serializable]
        public class Waypoint {
            public Vector2 localPosition;
        }

        private void Start() {
            RecordProperties();
        }

        [Header("State")]
        [ReadOnly][SerializeField] private int nextWaypointNumber;
        [ReadOnly] [SerializeField] private List<TopDownMovementController> characterContents;

        private Vector2 _startPosition;

        private Rigidbody2D _rigidbody;
        public Bounds colliderLocalBounds { get; private set; }

        private Vector2 _position;
        public Vector2 position {
            get {
                return _position;
            } set {
                Vector2 delta = value - _position;
                _position = value;
                _rigidbody.MovePosition(_position);

                foreach(var character in characterContents) {
                    if (character.currentFloor.GetComponentInParent<MovingPlatformController>() == this) {
                        character.characterMover.DisplaceAndSlide(delta * friction);
                    }
                }
            }
        }

        private void Update() {
            if (waypoints.Count < 1) {
                waypoints.Add(new Waypoint());
            }
            waypoints[0].localPosition = Vector2.zero;
            if (!Application.isPlaying) {
                RecordProperties();
            }
        }


        private void RecordProperties() {
            _startPosition = transform.position;

            var attachedColliders = new List<Collider2D>();
            _rigidbody.GetAttachedColliders(attachedColliders);
            Bounds? b = null;
            foreach (var collider in attachedColliders) {
                if (!b.HasValue) {
                    b = collider.bounds;
                } else {
                    var bounds = collider.bounds;
                    bounds.Encapsulate(b.Value);
                    b = bounds;
                }
            }
            if (b.HasValue) {
                var bounds = b.Value;
                bounds.center = transform.InverseTransformPoint(bounds.center);
                colliderLocalBounds = bounds;
            } else {
                colliderLocalBounds = new Bounds();
            }
        }

        private void FixedUpdate() {
            ClampWaypointNumber();

            var goalPoint = ToWorld(waypoints[nextWaypointNumber].localPosition);
            var currentPoint = _rigidbody.position;
            float maxDelta = Mathf.Abs(speed) * Time.fixedDeltaTime;
            var newPoint = Vector2.MoveTowards(currentPoint, goalPoint, maxDelta);
            if(newPoint == goalPoint) {
                nextWaypointNumber += reverse ? -1 : 1;
            } else {
                position = newPoint;
            }
        }

        private void ClampWaypointNumber() {
            switch (wrapMode) {
                case WrapMode.NONE:
                    nextWaypointNumber = Mathf.Clamp(nextWaypointNumber, 0, waypoints.Count - 1);
                    break;
                case WrapMode.LOOP:
                    if (nextWaypointNumber >= waypoints.Count) {
                        nextWaypointNumber = 0;
                    }
                    if (nextWaypointNumber < 0) {
                        nextWaypointNumber = waypoints.Count - 1;
                    }
                    break;
                case WrapMode.PINGPONG:
                    if (nextWaypointNumber >= waypoints.Count) {
                        nextWaypointNumber = waypoints.Count - 1;
                        reverse = !reverse;
                    }
                    if (nextWaypointNumber < 0) {
                        nextWaypointNumber = 0;
                        reverse = !reverse;
                    }
                    break;
            }
        }

        private void OnValidate() {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public Vector2 ToWorld(Vector2 point) {
            return (Vector2)transform.TransformVector(point) + _startPosition;
        }

        public Vector2 ToLocal(Vector2 point) {
            return transform.InverseTransformVector(point - _startPosition);
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            var character = collision.GetComponentInParent<TopDownMovementController>();
            if(character) {
                if(!characterContents.Contains(character)) {
                    characterContents.Add(character);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision) {
            var character = collision.GetComponentInParent<TopDownMovementController>();
            characterContents.Remove(character);
        }
    }
}
