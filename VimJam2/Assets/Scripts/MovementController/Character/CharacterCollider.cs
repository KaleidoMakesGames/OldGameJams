using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace KMGMovement2D {
    [System.Serializable]
    public class CharacterCollider {
        public enum ColliderType { Box, Circle, Capsule };
        [System.Serializable]
        public class ColliderGeometry {
            public ColliderType colliderType = ColliderType.Box;
            public Vector2 size = Vector2.one;
            public float radius = 0.5f;
            public CapsuleDirection2D direction;
        }
        public ColliderGeometry geometry;

        public float innerSkinWidth = 0.02f;
        public Vector2 offset;
        public LayerMask interactionMask = ~0;

        public enum ColliderSkin {INNER, NORMAL};
        
        public Vector2 colliderSize {
            get {
                return new Vector2(Mathf.Abs(geometry.size.x), Mathf.Abs(geometry.size.y));
            }
        }

        public float colliderRadius {
            get {
                return Mathf.Abs(geometry.radius);
            }
        }

        public CapsuleDirection2D colliderDirection {
            get {
                return geometry.direction;
            }
        }

        public Bounds bounds {
            get {
                switch(geometry.colliderType) {
                    case ColliderType.Box:
                        return new Bounds(offset, colliderSize);
                    case ColliderType.Capsule:
                        return new Bounds(offset, colliderSize);
                    case ColliderType.Circle:
                        return new Bounds(offset, Vector2.one * colliderRadius * 2.0f);
                }
                return new Bounds();
            }
        }
        
        public IEnumerable<RaycastHit2D> CastAll(Vector2 colliderPosition, float colliderAngle, Vector2 direction,  float distance, ColliderSkin colliderToUse) {
            float skinOffset = GetSkinWidthForDirection(direction, colliderToUse);
            IEnumerable<RaycastHit2D> hits = null;
            switch (geometry.colliderType) {
                case ColliderType.Box:
                    hits = RoundedBoxCastAll(colliderPosition + offset, GetSkinnedColliderSize(colliderToUse), GetSkinnedColliderRadius(colliderToUse), colliderAngle, direction.normalized, distance - skinOffset, interactionMask);
                    break;
                case ColliderType.Capsule:
                    hits = Physics2D.CapsuleCastAll(colliderPosition + offset, GetSkinnedColliderSize(colliderToUse), colliderDirection, colliderAngle, direction, distance - skinOffset, interactionMask);
                    break;
                case ColliderType.Circle:
                    hits = Physics2D.CircleCastAll(colliderPosition + offset, GetSkinnedColliderRadius(colliderToUse), direction.normalized, distance - skinOffset, interactionMask);
                    break;
                default:
                    Debug.LogError("Invalid or unset Collider2D (KMGMovement2D only supports CircleCollider2D, BoxCollider2D, and CapsuleCollider2D).");
                    return null;
            }
            return hits.Select(delegate (RaycastHit2D x) {
                x.distance += skinOffset;
                return x;
            });
        }

        public IEnumerable<Collider2D> OverlapAll(Vector2 colliderPosition, float colliderAngle, ColliderSkin colliderToUse) {
            switch (geometry.colliderType) {
                case ColliderType.Box:
                    return OverlapRoundedBoxAll(colliderPosition + offset, GetSkinnedColliderSize(colliderToUse), GetSkinnedColliderRadius(colliderToUse), colliderAngle, interactionMask);
                case ColliderType.Capsule:
                    return Physics2D.OverlapCapsuleAll(colliderPosition + offset, GetSkinnedColliderSize(colliderToUse), colliderDirection, colliderAngle, interactionMask);
                case ColliderType.Circle:
                    return Physics2D.OverlapCircleAll(colliderPosition + offset, GetSkinnedColliderRadius(colliderToUse), interactionMask);
                default:
                    Debug.LogError("Invalid or unset Collider2D (KMGMovement2D only supports CircleCollider2D, BoxCollider2D, and CapsuleCollider2D).");
                    return null;
            }
        }

        private float GetSkinWidthForDirection(Vector2 direction, ColliderSkin skin) {
            return GetNormSkinWidthForDirection(direction) * GetSkinWidth(skin);
        }

        public Vector2 GetSkinnedColliderSize(ColliderSkin skin) {
            float skinWidth = GetSkinWidth(skin);
            if(geometry.colliderType == ColliderType.Circle) {
                return colliderSize + Vector2.one * skinWidth;
            } else {
                return colliderSize + 2 * Vector2.one * skinWidth;
            }
        }

        public float GetSkinnedColliderRadius (ColliderSkin skin) {
            return Mathf.Max(0, colliderRadius + GetSkinWidth(skin));
        }

        private float GetSkinWidth(ColliderSkin skin) {
            return (skin == ColliderSkin.INNER ? -innerSkinWidth : 0);
        }


        private float GetNormSkinWidthForDirection(Vector2 direction) {
            if(geometry.colliderType != ColliderType.Box) {
                return 1.0f;
            }

            float upAngle = Vector2.Angle(Vector2.up, direction) * Mathf.Deg2Rad;
            float rightAngle = Vector2.Angle(Vector2.right, direction) * Mathf.Deg2Rad;

            float cosUp = Mathf.Abs(Mathf.Cos(upAngle));
            float cosRight = Mathf.Abs(Mathf.Cos(rightAngle));

            float upWidth = Mathf.Infinity;
            float rightWidth = Mathf.Infinity;
            if(cosUp > 0.0f) {
                upWidth = 1.0f / cosUp;
            }
            if(cosRight > 0.0f) {
                rightWidth = 1.0f / cosRight;
            }

            return Mathf.Min(rightWidth, upWidth);
        }


        public void DrawCollider(Vector2 position, float colliderAngle, Color color, bool debug=true, float duration = 0) {
            DrawColliderAtPosition(position, colliderAngle, color * 0.75f, ColliderSkin.INNER, debug, duration);
            DrawColliderAtPosition(position, colliderAngle, color, ColliderSkin.NORMAL, debug, duration);
        }

        public void DrawColliderAtPosition(Vector2 position, float colliderAngle, Color color, ColliderSkin colliderToUse, bool debug, float duration) {
            switch (geometry.colliderType) {
                case ColliderType.Box:
                    DrawingUtilities.DrawRectangle(position + offset, GetSkinnedColliderSize(colliderToUse), GetSkinnedColliderRadius(colliderToUse), colliderAngle, color, debug, duration);
                    break;
                case ColliderType.Capsule:
                    DrawingUtilities.DrawCapsule(position + offset, GetSkinnedColliderSize(colliderToUse), colliderDirection, colliderAngle, color, debug, duration);
                    break;
                case ColliderType.Circle:
                    DrawingUtilities.DrawCircle(position + offset, GetSkinnedColliderRadius(colliderToUse), color, 0.0f, 360.0f, debug, duration);
                    break;
            }
        }

        public void ValidateGeometry() {
            float minDim = Mathf.Min(geometry.size.x, geometry.size.y);
            geometry.size = new Vector2(Mathf.Clamp(geometry.size.x, 0.01f, Mathf.Infinity), Mathf.Clamp(geometry.size.y, 0.01f, Mathf.Infinity));
            switch (geometry.colliderType) {
                case ColliderType.Box:
                    geometry.radius = Mathf.Clamp(geometry.radius, 0, minDim/2.0f);
                    innerSkinWidth = Mathf.Clamp(innerSkinWidth, 0, minDim / 2.0f);
                    break;
                case ColliderType.Circle:
                    geometry.radius = Mathf.Clamp(geometry.radius, 0.01f, Mathf.Infinity);
                    innerSkinWidth = Mathf.Clamp(innerSkinWidth, 0, geometry.radius);
                    break;
                case ColliderType.Capsule:
                    innerSkinWidth = Mathf.Clamp(innerSkinWidth, 0, minDim / 2.0f);
                    break;
            }
        }

        public static IEnumerable<RaycastHit2D> RoundedBoxCastAll(Vector2 origin, Vector2 size, float radius, float angle, Vector2 direction, float distance, LayerMask layerMask) {
            if(radius == 0.0f) {
                return Physics2D.BoxCastAll(origin, size, angle, direction, distance, layerMask);
            }

            Vector2 verticalBoxSize = new Vector2(size.x - radius * 2.0f, size.y);
            Vector2 horizontalBoxSize = new Vector2(size.x, size.y - radius * 2.0f);

            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            Vector2 tlCircle = rotation * new Vector2(-size.x / 2 + radius, size.y / 2 - radius) + (Vector3)origin;
            Vector2 trCircle = rotation * new Vector2(size.x / 2 - radius, size.y / 2 - radius) + (Vector3)origin;
            Vector2 brCircle = rotation * new Vector2(size.x / 2 - radius, -size.y / 2 + radius) + (Vector3)origin;
            Vector2 blCircle = rotation * new Vector2(-size.x / 2 + radius, -size.y / 2 + radius) + (Vector3)origin;

            var verticalBoxHits = Physics2D.BoxCastAll(origin, verticalBoxSize, angle, direction, distance, layerMask).Where(x => x.collider.GetComponentInParent<CharacterMover2D>() == null);
            var horizontalBoxHits = Physics2D.BoxCastAll(origin, horizontalBoxSize, angle, direction, distance, layerMask).Where(x => x.collider.GetComponentInParent<CharacterMover2D>() == null);
            var tlHits = Physics2D.CircleCastAll(tlCircle, radius, direction, distance, layerMask).Where(x => x.collider.GetComponentInParent<CharacterMover2D>() == null);
            var trHits = Physics2D.CircleCastAll(trCircle, radius, direction, distance, layerMask).Where(x => x.collider.GetComponentInParent<CharacterMover2D>() == null);
            var blHits = Physics2D.CircleCastAll(blCircle, radius, direction, distance, layerMask).Where(x => x.collider.GetComponentInParent<CharacterMover2D>() == null);
            var brHits = Physics2D.CircleCastAll(brCircle, radius, direction, distance, layerMask).Where(x => x.collider.GetComponentInParent<CharacterMover2D>() == null);

            var verticalBoxSurfaceHits = verticalBoxHits.Where(delegate (RaycastHit2D hit) {
                Vector2 local = WorldToLocalDir(hit.normal, angle);
                float minAngle = Mathf.Min(Vector2.Angle(Vector2.up, local), Vector2.Angle(Vector2.down, local));
                bool isOnHorizontalSurface = minAngle < 10.0f;
                return isOnHorizontalSurface;
            });

            var horizontalBoxSurfaceHits = horizontalBoxHits.Where(delegate (RaycastHit2D hit) {
                Vector2 local = WorldToLocalDir(hit.normal, angle);
                float minAngle = Mathf.Min(Vector2.Angle(Vector2.right, local), Vector2.Angle(Vector2.left, local));
                bool isOnVerticalSurface = minAngle < 10.0f;
                return isOnVerticalSurface;
            });

            var tlSurfaceHits = tlHits.Where(delegate (RaycastHit2D hit) {
                Vector2 local = WorldToLocal(hit.point, tlCircle, angle);
                float localAngle = Vector2.Angle(new Vector2(-1, 1), local);
                return localAngle <= 45.0f;
            });
            var trSurfaceHits = trHits.Where(delegate (RaycastHit2D hit) {
                Vector2 local = WorldToLocal(hit.point, trCircle, angle);
                float localAngle = Vector2.Angle(new Vector2(1, 1), local);
                return localAngle <= 45.0f;
            });
            var blSurfaceHits = blHits.Where(delegate (RaycastHit2D hit) {
                Vector2 local = WorldToLocal(hit.point, blCircle, angle);
                float localAngle = Vector2.Angle(new Vector2(-1, -1), local);
                return localAngle <= 45.0f;
            });
            var brSurfaceHits = brHits.Where(delegate (RaycastHit2D hit) {
                Vector2 local = WorldToLocal(hit.point, brCircle, angle);
                float localAngle = Vector2.Angle(new Vector2(1, -1), local);
                return localAngle <= 45.0f;
            });

            return tlSurfaceHits
                .Concat(trSurfaceHits)
                .Concat(blSurfaceHits)
                .Concat(brSurfaceHits)
                .Concat(verticalBoxSurfaceHits)
                .Concat(horizontalBoxSurfaceHits).OrderBy(x => x.distance);
        }

        public static IEnumerable<Collider2D> OverlapRoundedBoxAll(Vector2 origin, Vector2 size, float radius, float angle, LayerMask layerMask) {
            if (radius == 0.0f) {
                return Physics2D.OverlapBoxAll(origin, size, angle, layerMask);
            }

            Vector2 verticalBoxSize = new Vector2(size.x - radius * 2.0f, size.y);
            Vector2 horizontalBoxSize = new Vector2(size.x, size.y - radius * 2.0f);

            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            Vector2 tlCircle = rotation * new Vector2(-size.x / 2 + radius, size.y / 2 - radius) + (Vector3)origin;
            Vector2 trCircle = rotation * new Vector2(size.x / 2 - radius, size.y / 2 - radius) + (Vector3)origin;
            Vector2 brCircle = rotation * new Vector2(size.x / 2 - radius, -size.y / 2 + radius) + (Vector3)origin;
            Vector2 blCircle = rotation * new Vector2(-size.x / 2 + radius, -size.y / 2 + radius) + (Vector3)origin;

            var verticalBoxHits = Physics2D.OverlapBoxAll(origin, verticalBoxSize, angle, layerMask);
            var horizontalBoxHits = Physics2D.OverlapBoxAll(origin, horizontalBoxSize, angle, layerMask);
            var tlHits = Physics2D.OverlapCircleAll(tlCircle, radius, layerMask);
            var trHits = Physics2D.OverlapCircleAll(trCircle, radius, layerMask);
            var blHits = Physics2D.OverlapCircleAll(blCircle, radius, layerMask);
            var brHits = Physics2D.OverlapCircleAll(brCircle, radius, layerMask);

            return verticalBoxHits
                .Union(horizontalBoxHits)
                .Union(tlHits)
                .Union(trHits)
                .Union(blHits)
                .Union(brHits);
        }
        public static Vector2 WorldToLocal(Vector2 point, Vector2 origin, float angle) {
            Quaternion inverseRotation = Quaternion.Euler(0, 0, -angle);
            return inverseRotation * (point - origin);
        }
        public static Vector2 WorldToLocalDir(Vector2 dir, float angle) {
            Quaternion inverseRotation = Quaternion.Euler(0, 0, -angle);
            return inverseRotation * dir;
        }
    }
}