using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackBehavior : ScriptableObject {
    public abstract void DoBehavior(CharacterAttackController controller);

    public virtual void DrawAttackGizmos(CharacterAttackController controller) { }
}
