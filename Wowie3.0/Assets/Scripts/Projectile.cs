using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Projectile : ScriptableObject
{
    [Header("Appearance")]
    public Sprite sprite;
    public bool alignUp;

    [Header("Behavior")]
    public int damage;
    public float size = 1.0f;
    public float maxTime = Mathf.Infinity;
}
