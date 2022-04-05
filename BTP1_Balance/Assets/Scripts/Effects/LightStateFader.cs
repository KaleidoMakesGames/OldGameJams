using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightStateFader : MonoBehaviour {
    private new Light light;

    public GameState onState;
    public GameStateTracker tracker;

    public float onIntensity;
    public float offIntensity;

    public float onSpeed;
    public float offSpeed;

    private float goalIntensity = 0.0f;

    private void Awake() {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update () {
        goalIntensity = (tracker == null || tracker.currentState == onState) ? onIntensity : offIntensity;

        float speed = tracker.currentState == onState ? onSpeed : offSpeed;

        light.intensity = Mathf.MoveTowards(light.intensity, goalIntensity, speed * Time.deltaTime);
	}
}
