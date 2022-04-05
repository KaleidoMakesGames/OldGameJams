using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character {
    [CreateAssetMenu(menuName ="BALANCE/Keyboard Movement Behavior")]
    public class KeyboardMovementBehavior : MovementBehavior {
        public KeyCode upKey;
        public KeyCode downKey;
        public KeyCode leftKey;
        public KeyCode rightKey;

        public override void DoBehavior(CharacterMovementController controller) {
            Vector2 moveDirection = Vector2.zero;

            moveDirection += Input.GetKey(upKey) ? Vector2.up : Vector2.zero;
            moveDirection += Input.GetKey(downKey) ? Vector2.down : Vector2.zero;
            moveDirection += Input.GetKey(leftKey) ? Vector2.left : Vector2.zero;
            moveDirection += Input.GetKey(rightKey) ? Vector2.right : Vector2.zero;

            controller.MoveInDirection(moveDirection);
        }
    }
}
