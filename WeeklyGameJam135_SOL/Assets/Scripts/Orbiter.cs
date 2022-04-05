using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Orbiter : MonoBehaviour
{
    public OrbitBody objectToOrbit;
    public float orbitHeightAboveSurface;

    public float orbitSpeed;

    public float angle;

    [HideInInspector] public UnityEvent onLoop;

    public float orbitRadius {
        get {
            return objectToOrbit.radius + orbitHeightAboveSurface;
        }
    }

    private void FixedUpdate() {
        if (objectToOrbit != null) {
            float angularSpeed = orbitSpeed / orbitRadius;

            float nextAngle = angle + angularSpeed * Time.fixedDeltaTime;
            if(nextAngle < 0.0f || nextAngle > 360.0f) {
                onLoop.Invoke();
            }
            angle = Mathf.Repeat(nextAngle, 360.0f);
            UpdatePosition();
        }
    }

    private void UpdatePosition() {
        if (objectToOrbit != null) {
            transform.position = objectToOrbit.transform.position;
            transform.localEulerAngles = new Vector3(0.0f, 0.0f, angle);
            transform.Translate(Vector2.up * orbitRadius, Space.Self);
        }
    }

    private void OnValidate() {
        UpdatePosition();
    }
}
