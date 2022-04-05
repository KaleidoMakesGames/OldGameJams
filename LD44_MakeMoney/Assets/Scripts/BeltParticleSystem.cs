using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltParticleSystem : MonoBehaviour
{
    public ParticleSystem system;

    public float trackDensity;  // bars per meter
    public float beltSpeed; // meters per second

    private void Update() {
        var emitter = system.emission;
        emitter.rateOverTime = new ParticleSystem.MinMaxCurve(trackDensity);
        var mainModule = system.main;
        mainModule.simulationSpeed = beltSpeed;
    }
}
