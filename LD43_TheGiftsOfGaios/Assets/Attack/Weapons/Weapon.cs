using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : ScriptableObject {
    public float cooldown;
    public float damage;

    public abstract void Use(CharacterAttackController attackController, Vector2 target);
    public virtual void DrawAttackGizmos(CharacterAttackController controller) { }
}
