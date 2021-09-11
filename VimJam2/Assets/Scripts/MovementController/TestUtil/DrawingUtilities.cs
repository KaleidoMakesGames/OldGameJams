using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KMGMovement2D {
    public class DrawingUtilities {
        public static void DrawCapsule(Vector2 position, Vector2 size, CapsuleDirection2D direction, float angle, Color color, bool debug, float duration) {
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            Vector2 colliderDirectionVector = rotation * (direction == CapsuleDirection2D.Vertical ? Vector2.up : Vector2.right);
            Vector2 colliderDirectionPerpendicularVector = rotation * (direction == CapsuleDirection2D.Vertical ? Vector2.right : Vector2.up);

            float radius = (direction == CapsuleDirection2D.Vertical ? size.x : size.y) / 2.0f;
            float boxHeight = 2 * Mathf.Max(0.0f, direction == CapsuleDirection2D.Vertical ? (size.y / 2.0f - radius) : (size.x / 2.0f - radius));

            Vector2 colliderCircleA = colliderDirectionVector * boxHeight / 2.0f + position;
            Vector2 colliderCircleB = -colliderDirectionVector * boxHeight / 2.0f + position;
            float circleAAngleStart = angle + (direction == CapsuleDirection2D.Vertical ? 0.0f : -90.0f);
            float circleAAngleEnd = angle + (direction == CapsuleDirection2D.Vertical ? 180.0f : 90.0f);
            float circleBAngleStart = angle + (direction == CapsuleDirection2D.Vertical ? 180.0f : 90.0f);
            float circleBAngleEnd = angle + (direction == CapsuleDirection2D.Vertical ? 360.0f : 270.0f);
            DrawCircle(colliderCircleA, radius, color, circleAAngleStart, circleAAngleEnd, debug, duration);
            DrawCircle(colliderCircleB, radius, color, circleBAngleStart, circleBAngleEnd, debug, duration);

            DrawLine(colliderDirectionPerpendicularVector * radius + colliderDirectionVector * boxHeight / 2.0f + position,
                        colliderDirectionPerpendicularVector * radius - colliderDirectionVector * boxHeight / 2.0f + position, color, debug, duration);
            DrawLine(-colliderDirectionPerpendicularVector * radius + colliderDirectionVector * boxHeight / 2.0f + position,
                        -colliderDirectionPerpendicularVector * radius - colliderDirectionVector * boxHeight / 2.0f + position, color, debug, duration);
        }

        public static void DrawLine(Vector2 a, Vector2 b, Color color, bool debug, float duration) {
            if (debug) {
                Debug.DrawLine(a, b, color, duration);
            } else {
                Gizmos.color = color;
                Gizmos.DrawLine(a, b);
            }
        }

        public static void DrawCircle(Vector2 position, float radius, Color color, float startAngle, float endAngle, bool debug, float duration) {
            int numPoints = 60;
            float degreesPerStep = 360.0f / numPoints;
            for (float angle = startAngle; angle < endAngle; angle += degreesPerStep) {
                float angleA = angle * Mathf.Deg2Rad;
                float angleB = (angle + degreesPerStep) * Mathf.Deg2Rad;
                Vector2 positionA = new Vector2(Mathf.Cos(angleA), Mathf.Sin(angleA)) * radius + position;
                Vector2 positionB = new Vector2(Mathf.Cos(angleB), Mathf.Sin(angleB)) * radius + position;
                DrawLine(positionA, positionB, color, debug, duration);
            }
        }

        public static void DrawRectangle(Vector2 position, Vector2 size, float radius, float angle, Color color, bool debug, float duration) {
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            Vector2 tlCircle = new Vector2(-size.x / 2 + radius, size.y / 2 - radius);
            Vector2 trCircle = new Vector2(size.x / 2 - radius, size.y / 2 - radius);
            Vector2 brCircle = new Vector2(size.x / 2 - radius, -size.y / 2 + radius);
            Vector2 blCircle = new Vector2(-size.x / 2 + radius, -size.y / 2 + radius);

            Vector2 tlBox = new Vector2(-size.x / 2 + radius, size.y / 2);
            Vector2 trBox = new Vector2(size.x / 2 - radius, size.y / 2);
            Vector2 rtBox = new Vector2(size.x / 2, size.y / 2-radius);
            Vector2 rbBox = new Vector2(size.x / 2, -size.y / 2+radius);
            Vector2 brBox = new Vector2(size.x / 2 - radius, -size.y / 2);
            Vector2 blBox = new Vector2(-size.x / 2 + radius, -size.y / 2);
            Vector2 lbBox = new Vector2(-size.x / 2, -size.y / 2+radius);
            Vector2 ltBox = new Vector2(-size.x / 2, size.y / 2-radius);


            DrawCircle(rotation * tlCircle + (Vector3)position, radius, color, 90+angle, 180+angle, debug, duration);
            DrawLine(rotation*tlBox + (Vector3)position, rotation*trBox + (Vector3)position, color, debug, duration);
            DrawCircle(rotation * trCircle + (Vector3)position, radius, color, angle, 90 + angle, debug, duration);
            DrawLine(rotation * rtBox + (Vector3)position, rotation * rbBox + (Vector3)position, color, debug, duration);
            DrawCircle(rotation * brCircle + (Vector3)position, radius, color, 270 + angle, 360 + angle, debug, duration);
            DrawLine(rotation * brBox + (Vector3)position, rotation * blBox + (Vector3)position, color, debug, duration);
            DrawCircle(rotation * blCircle + (Vector3)position, radius, color, 180 + angle, 270 + angle, debug, duration);
            DrawLine(rotation * lbBox + (Vector3)position, rotation * ltBox + (Vector3)position, color, debug, duration);
        }
    }
}