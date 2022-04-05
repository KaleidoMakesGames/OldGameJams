using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BALANCE/Weapons/Keyboard Weapon Behavior")]
public class KeyboardWeaponBehavior : WeaponBehavior {
    public KeyCode attackKey;
    public override void DoBehavior(WeaponController controller) {
        if(Input.GetKeyUp(attackKey)) {
            controller.UseWeapon();
        }
    }
}
