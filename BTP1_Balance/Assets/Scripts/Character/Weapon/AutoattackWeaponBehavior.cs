using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="BALANCE/Weapons/AA Weapon Behavior")]
public class AutoattackWeaponBehavior : WeaponBehavior {
    public bool alwaysAttack = false;
    public override void DoBehavior(WeaponController controller) {
        if(controller.AreEnemiesInRange() || alwaysAttack) {
            controller.UseWeapon();
        }
    }
}
