using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleScroller : MonoBehaviour
{
    public float distanceFromRunner;

    private ParticleSystem particles;

    private void Awake() {
        particles = GetComponent<ParticleSystem>();
        var particleModule = particles.main;
        particleModule.startSpeed = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        var particleModule = particles.main;
        particleModule.simulationSpeed = Runner.Instance.enabled ? (Runner.Instance.runningVector.x * (1 / (distanceFromRunner + 1.0f))) : 0.0f;
    }

    private void FixedUpdate() {
        Vector2 movement = -Runner.Instance.runningVector * (1 / (distanceFromRunner + 1.0f)) * Time.fixedDeltaTime;
        movement.x = 0.0f;
        transform.Translate(movement);
    }
}
