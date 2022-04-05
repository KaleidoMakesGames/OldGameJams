using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltController : MonoBehaviour {
    public float speed;
    public float speedDelta;
    public float maxSpeed;
    public float minSpeed;
    public float toyDensity;

    public BeltEffector beltEffector;
    public ToySpawner spawner;
    public BeltParticleSystem particles;

    private void Awake() {
        UpdateSpeed();    
    }

    private void Update() {
        UpdateSpeed();
    }

    private void UpdateSpeed() { 
        beltEffector.speed = speed;
        spawner.toysPerSecond = toyDensity* speed;
        particles.beltSpeed = speed;
    }

    public void SpeedUp() {
        SetSpeed(speed + speedDelta);
    }

    public void SlowDown() {
        SetSpeed(speed - speedDelta);
    }

    public void SetSpeed(float newSpeed) {
        speed = Mathf.Clamp(newSpeed, minSpeed, maxSpeed);
    }
}
