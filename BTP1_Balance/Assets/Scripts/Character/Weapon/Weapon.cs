using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="BALANCE/Weapons/Weapon")]
public class Weapon : ScriptableObject {
    public float damage;
    public float range;
    public float cooldown;
    public float force;
}
