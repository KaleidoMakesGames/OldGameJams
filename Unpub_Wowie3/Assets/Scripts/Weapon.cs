using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Weapon : ScriptableObject {
    [Header("Projectile")]
    public Projectile projectileToShoot;
    public float speedMultiplier;
    public AnimationCurve speedOverTime;

    [Header("Fire Dynamics")]
    public float fireDelay;
    public int projectilesPerBurst;
    public float burstDelay;
    public bool automatic;

    [Header("Clip")]
    public int clipSize;
    
}
