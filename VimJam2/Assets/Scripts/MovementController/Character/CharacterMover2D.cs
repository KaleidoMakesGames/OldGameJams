using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

namespace KMGMovement2D {
    [ExecuteAlways]
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterMover2D : MonoBehaviour {
        public CharacterCollider characterCollider;

        [Header("Sliding")]
        public SlideMode slideMode = SlideMode.Projected;
        [Range(0, 1)] public float slideAmount = 1.0f;

        public Vector2 characterPosition { get; private set; }

        [Header("Debug")]
        public bool debugMode;
        public float debugTime;
        public int drawEvery;
        [ReadOnly][SerializeField]private int _currentStep;

        public float characterAngle {
            get {
                return transform.eulerAngles.z;
            }
        }

        private CircleCollider2D _circleCollider2D;
        private CapsuleCollider2D _capsuleCollider2D;
        private BoxCollider2D _boxCollider2D;
        private Rigidbody2D _rigidbody2D;

        [HideInInspector][SerializeField] public UnityEvent OnDebugReset;

        private void Start() {
            GetReferences();
            SyncColliders();
            characterPosition = _rigidbody2D.position;
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

#if UNITY_EDITOR
            _circleCollider2D.hideFlags = HideFlags.HideInInspector;
            _capsuleCollider2D.hideFlags = HideFlags.HideInInspector;
            _boxCollider2D.hideFlags = HideFlags.HideInInspector;
            _rigidbody2D.hideFlags = HideFlags.HideInInspector;
#endif
        }
        

        private void OnDrawGizmosSelected() {
            characterCollider.DrawCollider(transform.position, transform.eulerAngles.z, Color.green, false);
        }

        private void FixedUpdate() {
            if (debugMode) {
                _currentStep++;
                if (_currentStep >= debugTime / Time.fixedDeltaTime) {
                    _currentStep = 0;
                    characterPosition = _rigidbody2D.position;
                    OnDebugReset.Invoke();
                }
                if (drawEvery > 0 && _currentStep % drawEvery == 0) {
                    characterCollider.DrawCollider(characterPosition, characterAngle, Color.white, true, Time.fixedDeltaTime * drawEvery);
                }
            } else {
                if (IsPositionBlocked(characterPosition)) {
                    Debug.LogError("Collision constraint violated!");
                }
            }
        }

        private void Reset() {
            GetReferences();
            SyncColliders();
        }

        private void Update() {
            if (transform.lossyScale != Vector3.one) {
                Debug.LogError("Lossy scale must be 1.");
            }
        }

        private void OnValidate() {
            GetReferences();
            characterCollider.ValidateGeometry();
            SyncColliders();
        }

        public enum SlideMode { Projected, Proportional }
        public RaycastHit2D DisplaceAndSlide(Vector2 displacement) {
            var lastObstacle = new RaycastHit2D();
            int step = 0;
            Vector2 currentDisplacement = displacement;
            for(; step < 8; step++) {
                if (displacement.magnitude == 0) {
                    break;
                }
                var obstacle = Displace(currentDisplacement);
                if(obstacle) {
                    if (debugMode) {
                        Debug.DrawLine(characterPosition, characterPosition + obstacle.normal, Color.blue);
                        Debug.DrawLine(characterPosition, characterPosition + currentDisplacement.normalized, Color.Lerp(Color.blue, Color.white, 0.5f));
                    }
                    lastObstacle = obstacle;
                    float remainingDisplacement = Mathf.Max(0, currentDisplacement.magnitude - obstacle.distance);
                    Vector2 surface = Vector2.Perpendicular(obstacle.normal).normalized;
                    Vector2 projected = Vector2.Dot(displacement.normalized * remainingDisplacement, surface) * surface;
                    if (slideMode == SlideMode.Projected) {
                        currentDisplacement = projected;
                    } else {
                        currentDisplacement = projected.normalized * remainingDisplacement;
                    }
                    currentDisplacement *= slideAmount;
                } else {
                    break;
                }
            }
            return lastObstacle;
        }

        public RaycastHit2D TryMove(Vector2 position, bool slide = false) {
            if (slide) {
                return DisplaceAndSlide(position - characterPosition);
            } else {
                return Displace(position - characterPosition);
            }
        }

        public RaycastHit2D Displace(Vector2 displacement) {
            var firstHit = FirstObstacle(displacement);
            if (firstHit) {
                Move(characterPosition + displacement.normalized * firstHit.distance, false);
            } else {
                Move(characterPosition + displacement, false);
            }
            return new RaycastHit2D();
        }

        public RaycastHit2D FirstObstacle(Vector2 displacement) {
            var forwardCast = characterCollider.CastAll(characterPosition, characterAngle, displacement.normalized, displacement.magnitude, CharacterCollider.ColliderSkin.INNER);
            foreach (var hit in forwardCast) {
                if (IsObstacle(hit.collider)) {
                    return hit;
                }
            }
            return new RaycastHit2D();
        }

        public RaycastHit2D Teleport(Vector2 position, bool moveTowardsOnFail = false) {
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

        public void Move(Vector2 position, bool teleport) {
            characterPosition = position;
            if (!debugMode) {
                if (teleport) {
                    _rigidbody2D.position = position;
                    transform.position = position;
                } else {
                    _rigidbody2D.MovePosition(position);
                }
            }
        }

        public bool IsPositionBlocked(Vector2 position) {
            var destinationCast = characterCollider.OverlapAll(position, characterAngle, CharacterCollider.ColliderSkin.INNER);
            return destinationCast.Any(IsObstacle);
        }

        public IEnumerable<RaycastHit2D> GetTouchingObstacles() {
            return GetTouchingObstacles(characterPosition);
        }

        public IEnumerable<RaycastHit2D> GetTouchingObstacles(Vector2 position) {
            var touchingObjects = characterCollider.CastAll(position, characterAngle, Vector2.zero, 0.0f, CharacterCollider.ColliderSkin.NORMAL);
            return touchingObjects.Where(x => x.collider.GetComponentInParent<CharacterMover2D>() != this);
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