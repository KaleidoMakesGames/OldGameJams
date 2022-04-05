using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character {
    public abstract class MovementBehavior : ScriptableObject {
        public abstract void DoBehavior(CharacterMovementController controller);
    }
}
