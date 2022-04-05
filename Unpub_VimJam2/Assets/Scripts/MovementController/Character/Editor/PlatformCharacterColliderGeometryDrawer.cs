using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace KMGMovement2D {
    [CustomPropertyDrawer(typeof(CharacterCollider.ColliderGeometry))]
    public class PlatformerCharacterColliderGeometryDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            SerializedProperty colliderTypeProperty = property.FindPropertyRelative("colliderType");
            SerializedProperty sizeProperty = property.FindPropertyRelative("size");
            SerializedProperty radiusProperty = property.FindPropertyRelative("radius");
            SerializedProperty directionProperty = property.FindPropertyRelative("direction");

            EditorGUI.BeginProperty(position, label, property);

            Rect rect = new Rect(position.position, new Vector2(position.width, 0.0f));

            DrawField(colliderTypeProperty, ref rect);
            var colliderType = (CharacterCollider.ColliderType)colliderTypeProperty.enumValueIndex;

            EditorGUI.indentLevel += 1;
            if (colliderType == CharacterCollider.ColliderType.Box ||
                colliderType == CharacterCollider.ColliderType.Capsule) {
                DrawField(sizeProperty, ref rect);
            }
            if (colliderType == CharacterCollider.ColliderType.Circle ||
                colliderType == CharacterCollider.ColliderType.Box) {
                DrawField(radiusProperty, ref rect);
            }
            if (colliderType == CharacterCollider.ColliderType.Capsule) {
                DrawField(directionProperty, ref rect);
            }
        }

        private void DrawField(SerializedProperty property, ref Rect rect) {
            float height = EditorGUI.GetPropertyHeight(property);
            rect.height = height;
            EditorGUI.PropertyField(rect, property);
            rect.position = new Vector2(rect.position.x, rect.position.y + height + 4);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            float baseHeight = base.GetPropertyHeight(property, label) + 4;

            SerializedProperty colliderTypeProperty = property.FindPropertyRelative("colliderType");
            SerializedProperty sizeProperty = property.FindPropertyRelative("size");
            SerializedProperty radiusProperty = property.FindPropertyRelative("radius");
            SerializedProperty directionProperty = property.FindPropertyRelative("direction");

            var colliderType = (CharacterCollider.ColliderType)colliderTypeProperty.enumValueIndex;
            if (colliderType == CharacterCollider.ColliderType.Box ||
                colliderType == CharacterCollider.ColliderType.Capsule) {
                baseHeight += EditorGUI.GetPropertyHeight(sizeProperty) + 4;
            }
            if (colliderType == CharacterCollider.ColliderType.Capsule) {
                baseHeight += EditorGUI.GetPropertyHeight(directionProperty) + 4;
            }
            if (colliderType == CharacterCollider.ColliderType.Circle ||
                colliderType == CharacterCollider.ColliderType.Box) {
                baseHeight += EditorGUI.GetPropertyHeight(radiusProperty) + 4;
            }
            return baseHeight;
        }
    }
}