using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="GoG/Attack Behavior/Input Attack Behavior")]
public class InputAttackBehavior : AttackBehavior {
    public bool leftMouseButton;
    public bool rightMouseButton;
    public List<KeyCode> keys;

    public override void DoBehavior(CharacterAttackController controller) {
        Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if(ShouldUseWeapon()) {
            controller.Attack(target);
        }
    }

    public bool ShouldUseWeapon() {
        foreach (KeyCode key in keys) {
            if (Input.GetKey(key)) {
                return true;
            }
        }

        if((leftMouseButton && Input.GetMouseButton(0)) || (rightMouseButton && Input.GetMouseButton(1))) {
            return true;
        }
        return false;
    }
}
