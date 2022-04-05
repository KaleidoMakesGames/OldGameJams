using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TeamAssigner))]
public class CharacterAttackController : MonoBehaviour {
    public Weapon weapon;
    public AttackBehavior behavior;

    public Transform attackBasePoint;

    public bool canAttack = false;

    private float lastWeaponUseTime = Mathf.NegativeInfinity;

    public bool IsWeaponOnCooldown() {
        if (weapon == null) {
            return true;
        } else {
            if (Time.time - lastWeaponUseTime >= weapon.cooldown) {
                return false;
            } else {
                return true;
            }
        }
    }

    // Update is called once per frame
    void Update () {
        if(behavior != null) {
            behavior.DoBehavior(this);
        }
	}

    public void Attack(Vector2 target) {
        if(!canAttack) {
            return;
        }
        if(weapon != null && !IsWeaponOnCooldown()) {
            weapon.Use(this, target);
            lastWeaponUseTime = Time.time;
        }
    }

    private void OnDrawGizmosSelected() {
        if(behavior != null) {
            behavior.DrawAttackGizmos(this);
            weapon.DrawAttackGizmos(this);
        }
    }
}
