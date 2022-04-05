using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FlaskFiller : MonoBehaviour
{
    public Potion potion;

    private void Update() {
        foreach(ParticleSystem p in GetComponentsInChildren<ParticleSystem>()) {
            var t = p.trails;
            var c = t.colorOverLifetime;
            c.color = potion.friendlyColor;
            t.colorOverLifetime = c;
        }
    }
}
