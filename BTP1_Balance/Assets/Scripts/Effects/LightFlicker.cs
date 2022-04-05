using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour {
    public float flickerSpeed;

    public float flickerMax;
    public float flickerMin;

    private new Light light;
    
    private void Awake() {
        light = GetComponent<Light>();
    }

    private void OnEnable() {
        StartCoroutine(Flicker());
    }

    private IEnumerator Flicker() {
        while(true) {
            light.intensity = Random.Range(flickerMin, flickerMax);
            yield return new WaitForSeconds(1 / flickerSpeed);
        }
    }

    private void OnValidate() {
        flickerSpeed = Mathf.Clamp(flickerSpeed, 0.01f, Mathf.Infinity);
    }
}
