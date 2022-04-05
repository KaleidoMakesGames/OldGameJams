using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBehavior : ScriptableObject {
    public abstract void DoBehavior(WeaponController controller);
}
