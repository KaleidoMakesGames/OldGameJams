using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace KMGMovement2D {

    public interface IMovementUser {
        void DoTick();
        void DebugReset();
    }
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterMover2D : MonoBehaviour {
        public CharacterCollider characterCollider;

        public Vector2 characterPosition { get; private set; }

        public enum DebugMode { None, Low, High }
        [Header("Debug")]
        public DebugMode debugMode = DebugMode.None;
        public int numFrames;
        public float intermediateFrequency = 2.0f;
        public float updateFrequency = 2.0f;

        public float characterAngle {
            get {
                return transform.eulerAngles.z;
            }
        }

        private List<IMovementUser> users {
            get {
                return new List<IMovementUser>(GetComponents<IMovementUser>());
            }
        }

        private CircleCollider2D _circleCollider2D;
        private CapsuleCollider2D _capsuleCollider2D;
        private BoxCollider2D _boxCollider2D;
        private Rigidbody2D _rigidbody2D;


        private struct DebugInfo {
            public Color color;
            public string text;
            public Vector3 position;
        }
        private List<DebugInfo> _frameInfo;
        private List<DebugInfo> _intermediateInfo;

        private void Awake() {
            GetReferences();
            SyncColliders();
            characterPosition = _rigidbody2D.position;
        }

        private void FixedUpdate() {
            if(debugMode == DebugMode.None) {
                users.ForEach(x => x.DoTick());
            }
        }

        private void GetReferences() {
            _circleCollider2D = GetComponent<CircleCollider2D>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void SyncColliders() {
            _circleCollider2D.radius = characterCollider.GetSkinnedColliderRadius(CharacterCollider.ColliderSkin.INNER);
            _boxCollider2D.edgeRadius = characterCollider.GetSkinnedColliderRadius(CharacterCollider.ColliderSkin.INNER);
            _boxCollider2D.size = characterCollider.GetSkinnedColliderSize(CharacterCollider.ColliderSkin.INNER) - Vector2.one * _boxCollider2D.edgeRadius * 2;
            _capsuleCollider2D.size = characterCollider.GetSkinnedColliderSize(CharacterCollider.ColliderSkin.INNER);
            _capsuleCollider2D.direction = characterCollider.geometry.direction;

            _circleCollider2D.offset = characterCollider.offset;
            _boxCollider2D.offset = characterCollider.offset;
            _capsuleCollider2D.offset = characterCollider.offset;

            _circleCollider2D.enabled = characterCollider.geometry.colliderType == CharacterCollider.ColliderType.Circle;
            _boxCollider2D.enabled = characterCollider.geometry.colliderType == CharacterCollider.ColliderType.Box;
            _capsuleCollider2D.enabled = characterCollider.geometry.colliderType == CharacterCollider.ColliderType.Capsule;

            _rigidbody2D.isKinematic = true;
            _rigidbody2D.interpolation = RigidbodyInterpolation2D.Interpolate;
            _rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            var _hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
            _circleCollider2D.hideFlags = _hideFlags;
            _capsuleCollider2D.hideFlags = _hideFlags;
            _boxCollider2D.hideFlags = _hideFlags;
            _rigidbody2D.hideFlags = _hideFlags;
        }
        

        private void OnDrawGizmosSelected() {
            characterCollider.DrawCollider(transform.position, transform.eulerAngles.z, Color.green, false);
            if (debugMode != DebugMode.None && _frameInfo != null && _frameInfo.Count > 0) {
                DrawDebug(_frameInfo);
            }
            if (debugMode == DebugMode.High && _intermediateInfo != null && _intermediateInfo.Count > 0) {
                DrawDebug(_intermediateInfo, true);
            }
        }

        private void DrawDebug(List<DebugInfo> _debugInfo, bool doAnimation = false) {
#if UNITY_EDITOR
            if (debugMode != DebugMode.None && _debugInfo != null) {
                var style = new GUIStyle();
                style.alignment = TextAnchor.MiddleCenter;
                if(doAnimation) {
                    int frame = Mathf.FloorToInt(Time.time * intermediateFrequency) % _debugInfo.Count;
                    var info = _debugInfo[frame];
                    characterCollider.DrawCollider(info.position, 0, info.color, false);
                    Handles.color = info.color;
                    Handles.Label(info.position + (Vector3)characterCollider.offset, info.text, style);
                    return;
                }
                foreach (var info in _debugInfo) {
                    characterCollider.DrawCollider(info.position, 0, info.color, false);
                    Handles.color = info.color;
                    Handles.Label(info.position + (Vector3)characterCollider.offset, info.text, style);
                }
            }
#endif
        }

        public void Simulate() {
            _frameInfo = new List<DebugInfo>();
            _intermediateInfo = new List<DebugInfo>();
            characterPosition = transform.position;
            users.ForEach(x => x.DebugReset());
            for(int step = 0; step < numFrames; step++) {
                users.ForEach(x => x.DoTick());
                _RecordDebug(Color.white, "Frame " + step, _frameInfo);
            }
        }

        public void RecordDebug(Color color, string text, Vector2? position = null) {
            if(debugMode == DebugMode.High) {
                _RecordDebug(color, text, _intermediateInfo, position);
            }
        }

        private void _RecordDebug(Color color, string text, List<DebugInfo> infoList, Vector2? position = null) {
            var info = new DebugInfo();
            info.color = color;
            info.position = position.HasValue ? position.Value : characterPosition;
            info.text = text;
            infoList.Add(info);
        }

        private float _lastSimTime = Mathf.NegativeInfinity;
        private void Update() {
            if (transform.lossyScale != Vector3.one) {
                Debug.LogError("Lossy scale must be 1.");
            }
            if (debugMode != DebugMode.None) {
                if (Time.time - _lastSimTime >= Mathf.Max(0.0f, 1 / updateFrequency)) {
                    Simulate();
                    _lastSimTime = Time.time;
                }
            } else {
                _lastSimTime = Mathf.NegativeInfinity;
            }
        }

        private void OnValidate() {
            GetReferences();
            characterCollider.ValidateGeometry();
            SyncColliders();
        }

        public delegate void SlideHitDelegate(RaycastHit2D hit, ref Vector2 remainingDisplacement);

        public void DisplaceAndSlideTo(Vector2 position, SlideHitDelegate OnSlideHit = null, bool projectDisplacement = true) {
            DisplaceAndSlide(position - characterPosition, (position - characterPosition).magnitude, OnSlideHit, projectDisplacement);
        }

        public void DisplaceAndSlide(Vector2 direction, float distance, SlideHitDelegate OnSlideHit = null, bool projectDisplacement = true) {
            Vector2 remainingDisplacement = direction.normalized * distance;
            for (int step = 0; step < 10; step++) {
                if (remainingDisplacement.magnitude == 0) {
                    break;
                }
                var obstacle = FirstObstacle(remainingDisplacement.normalized, remainingDisplacement.magnitude);
                var clearing = FirstClearing(remainingDisplacement.normalized, remainingDisplacement.magnitude);

                bool useObstacle = obstacle && (!clearing || obstacle.distance < clearing.distance);
                bool useClearing = !useObstacle && clearing;

                if (useObstacle) {
                    Move(characterPosition + remainingDisplacement.normalized * obstacle.distance, false);
                    RecordDebug(Color.red, "Obstacle");
                    remainingDisplacement = Mathf.Max(0, remainingDisplacement.magnitude - obstacle.distance) * remainingDisplacement.normalized;
                    if (projectDisplacement) {
                        remainingDisplacement = Vector3.ProjectOnPlane(remainingDisplacement, obstacle.normal);
                    }
                    if(OnSlideHit != null) {
                        OnSlideHit(obstacle, ref remainingDisplacement);
                    }
                } else if (useClearing) {
                    RecordDebug(Color.yellow, "Cleared");
                    Move(characterPosition + remainingDisplacement.normalized * clearing.distance, false);
                    remainingDisplacement = Mathf.Max(0, remainingDisplacement.magnitude - clearing.distance) * direction.normalized;
                } else {
                    Move(characterPosition + remainingDisplacement);
                    remainingDisplacement = Vector2.zero;
                }
            }
        }

        public RaycastHit2D DisplaceTo(Vector2 position) {
            return Displace(position - characterPosition, (position - characterPosition).magnitude);
        }

        public RaycastHit2D FirstObstacle(Vector2 direction, float distance) {
            var forwardCast = characterCollider.CastAll(characterPosition, characterAngle, direction.normalized, distance, CharacterCollider.ColliderSkin.INNER);
            foreach (var hit in forwardCast) {
                if (IsObstacle(hit.collider)) {
                    return hit;
                }
            }
            return new RaycastHit2D();
        }
        public RaycastHit2D FirstClearing(Vector2 direction, float distance) {
            Vector2 delta = direction.normalized * distance;
            Vector2 endPoint = characterPosition + delta;
            var reverseCast = characterCollider.CastAll(endPoint, characterAngle, -direction.normalized, distance, CharacterCollider.ColliderSkin.NORMAL).Reverse();
            foreach (var hit in reverseCast) {
                if (IsObstacle(hit.collider)) {
                    float hitDistance = Mathf.Max(0, hit.distance - characterCollider.innerSkinWidth * 2.0f);
                    Vector2 possiblePosition = endPoint - direction.normalized * hitDistance;
                    if (!IsPositionBlocked(possiblePosition, CharacterCollider.ColliderSkin.NORMAL)) {
                        var flipHit = hit;
                        flipHit.distance = Vector2.Distance(characterPosition, possiblePosition);
                        return flipHit;
                    }
                }
            }
            return new RaycastHit2D();
        }

        public RaycastHit2D Displace(Vector2 direction, float distance) {
            var firstObstacle = FirstObstacle(direction, distance);
            if (firstObstacle) {
                Move(characterPosition + direction.normalized * firstObstacle.distance, false);
            } else {
                Move(characterPosition + direction.normalized * distance, false);
            }
            return firstObstacle;
        }

        public RaycastHit2D Teleport(Vector2 position, bool moveTowardsOnFail = true) {
            Vector2 destination = position;
            if (IsPositionBlocked(destination)) {
                if (moveTowardsOnFail) {
                    Vector2 displacement = position - characterPosition;
                    var forwardCast = characterCollider.CastAll(characterPosition, characterAngle, displacement.normalized, displacement.magnitude, CharacterCollider.ColliderSkin.INNER);
                    foreach (var hit in forwardCast.Reverse()) {
                        Vector2 possiblePosition = characterPosition + displacement.normalized * hit.distance;
                        if (!IsPositionBlocked(possiblePosition)) {
                            Move(possiblePosition, true);
                            return hit;
                        }
                    }
                }
                return new RaycastHit2D();
            } else {
                Move(destination, true);
                return new RaycastHit2D();
            }
        }

        public void Move(Vector2 position, bool teleport=false) {
            characterPosition = position;
            if (debugMode == DebugMode.None) {
                if (teleport) {
                    _rigidbody2D.position = position;
                    transform.position = position;
                } else {
                    _rigidbody2D.MovePosition(position);
                }
            }
        }

        public bool IsPositionBlocked(Vector2 position, CharacterCollider.ColliderSkin colliderToUse = CharacterCollider.ColliderSkin.INNER) {
            var destinationCast = characterCollider.OverlapAll(position, characterAngle, colliderToUse);
            return destinationCast.Any(IsObstacle);
        }

        public IEnumerable<Collider2D> GetTouchingObjects() {
            return GetTouchingObjects(characterPosition);
        }

        public IEnumerable<Collider2D> GetTouchingObjects(Vector2 position) {
            var touchingObjects = characterCollider.OverlapAll(position, characterAngle, CharacterCollider.ColliderSkin.NORMAL);
            return touchingObjects.Where(x => x.GetComponentInParent<CharacterMover2D>() != this);
        }

        public bool IsObstacle(Collider2D collider) {
            if(collider.GetComponentInParent<CharacterMover2D>() == this) {
                return false;
            }
            var scriptableObstacles = collider.GetComponentsInParent<IScriptableObstacle>();
            if (scriptableObstacles.Any()) {
                return scriptableObstacles.Any(x => x.IsObstacle(this));
            }

            if(collider.isTrigger) {
                return false;
            }

            return true;
        }
    }
}