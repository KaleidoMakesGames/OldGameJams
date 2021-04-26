using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialAnimator : MonoBehaviour
{
    public Material damagedMaterial;
    public List<MeshRenderer> meshRenderers;

    public float flashTime;

    private Dictionary<MeshRenderer, Material> _initialMaterials;

    private float _lastDamageTime;

    [ReadOnly]public bool running = false;

    public void Damage() {
        _lastDamageTime = Time.time;
        if(!running) {
            StartCoroutine(DoDamage());
        }
    }

    IEnumerator DoDamage() {
        running = true;
        _initialMaterials = new Dictionary<MeshRenderer, Material>();
        foreach(var mr in meshRenderers) {
            _initialMaterials[mr] = mr.material;
            mr.material = damagedMaterial;
        }
        while (Time.time - _lastDamageTime <= flashTime) {
            yield return null;
        }
        foreach(var mr in meshRenderers) {
            mr.material = _initialMaterials[mr];
        }
        running = false;
    }
}
