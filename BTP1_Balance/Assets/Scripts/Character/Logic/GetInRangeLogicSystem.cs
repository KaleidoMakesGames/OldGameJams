using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Logic {
    [CreateAssetMenu(menuName = "BALANCE/Logic/Get In Range System")]
    public class GetInRangeLogicSystem : LogicSystem {
        public float overlap;
        public override void UpdateLogic(CharacterLogicController controller) {
            WeaponController weaponController = controller.GetComponent<WeaponController>();
            NavigationTargeter targeter = controller.GetComponent<NavigationTargeter>();
            if(weaponController != null && targeter != null && weaponController.currentWeapon != null) {
                targeter.goalStoppingDistance = weaponController.currentWeapon.range - overlap;
            }
        }
    }
}
