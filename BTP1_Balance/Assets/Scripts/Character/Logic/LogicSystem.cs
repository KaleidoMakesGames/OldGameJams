using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Logic {
    public abstract class LogicSystem : ScriptableObject {
        public abstract void UpdateLogic(CharacterLogicController controller);

        public virtual void DrawLogicGizmos(CharacterLogicController controller) {

        }
    }
}
