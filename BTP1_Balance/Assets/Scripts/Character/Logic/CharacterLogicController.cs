using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Logic {
    public class CharacterLogicController : MonoBehaviour {
        public List<LogicSystem> logicSystems;

        private void Update() {
            foreach(LogicSystem system in logicSystems) {
                if (system != null) {
                    system.UpdateLogic(this);
                }
            }
        }

        private void OnDrawGizmos() {
            foreach (LogicSystem system in logicSystems) {
                if (system != null) {
                    system.DrawLogicGizmos(this);
                }
            }
        }
    }
}
