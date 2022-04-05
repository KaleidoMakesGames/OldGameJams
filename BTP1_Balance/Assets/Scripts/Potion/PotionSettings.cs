using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BALANCE/Potion Settings")]
public class PotionSettings : ScriptableObject {
    public float healAmount;

    public float size;

    public Team teamToHeal;
}
