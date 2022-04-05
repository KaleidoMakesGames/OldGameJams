using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBlinker : MonoBehaviour
{
    [ColorUsage(true, true)] public Color blinkColor;
    public MeshRenderer shieldRenderer;

    [ReadOnly] public bool running = false;

    public float flashTime;

    private float _lastDamageTime;

    private Color? lastColor;

    private Vector3 goalScale;

    public float growTime;

    private void Awake() {
        goalScale = transform.localScale;
    }

    private void OnDisable() {
        if (lastColor.HasValue) {
            shieldRenderer.material.SetColor("_SHIELDCOLOR", lastColor.Value);
            lastColor = null;
            running = false;
        }
    }

    private void OnEnable() {
        transform.localScale = Vector3.zero;
    }

    private void Update() {
        transform.localScale = Vector3.MoveTowards(transform.localScale, goalScale, growTime * Time.deltaTime);
    }

    public void DamageHealth() {
        _lastDamageTime = Time.time;
        if (!running) {
            StartCoroutine(DoDamage());
        }
    }

    IEnumerator DoDamage() {
        running = true;
        lastColor = shieldRenderer.material.GetColor("_SHIELDCOLOR");
        shieldRenderer.material.SetColor("_SHIELDCOLOR", blinkColor);

        while (Time.time - _lastDamageTime <= flashTime) {
            yield return null;
        }
        shieldRenderer.material.SetColor("_SHIELDCOLOR", lastColor.Value);
        lastColor = null;
        running = false;
    }
}
