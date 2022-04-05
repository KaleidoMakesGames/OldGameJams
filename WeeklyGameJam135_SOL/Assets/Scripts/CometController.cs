using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class CometController : MonoBehaviour
{
    public enum State { Idle, InOrbit, Launched, FreeAtLast}
    public State currentState;

    public Transform startPos;
    public float launchVelocity;
    public float baseVelocity;
    public float maxVelocity;

    public float rampRate;
    public float decayRate;

    public Orbiter orbiter;

    public OrbitBody massiveBody;
    public float massiveGravity;

    public ParticleSystem ps;
    
    public Cinemachine.CinemachineVirtualCamera cometCam;
    public float zoomSizeFactor;
    public float zoomSpeedFactor;

    public WishTracker tracker;

    private Rigidbody2D rb;

    public float speedProgress { get; set; }

    public float emissionRateProp;

    public float wishVelocity;

    public float launchedZoom;

    public float cometCamDistance {
        get {
            Cinemachine.CinemachineFramingTransposer t = cometCam.GetCinemachineComponent<Cinemachine.CinemachineFramingTransposer>();
            return t.m_CameraDistance;
        }
        set {
            Cinemachine.CinemachineFramingTransposer t = cometCam.GetCinemachineComponent<Cinemachine.CinemachineFramingTransposer>();
            t.m_CameraDistance = value;
        }
    }

    public UnityEvent OnLaunch;
    public UnityEvent OnLand;
    public UnityEvent OnWish;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        currentState = State.Idle;
        rb.isKinematic = true;
        speedProgress = 0;
    }

    private void Update() {
        if (currentState == State.InOrbit) {
            cometCam.Follow = orbiter.objectToOrbit.transform;
            if (Input.GetKey(KeyCode.Space)) {
                speedProgress = Mathf.Clamp01(speedProgress + rampRate * Time.deltaTime);
            } else {
                speedProgress = Mathf.Clamp01(speedProgress - decayRate * Time.deltaTime);
            }

            float linearVelocity = Mathf.SmoothStep(baseVelocity, maxVelocity, speedProgress);

            orbiter.orbitSpeed = Mathf.Sign(orbiter.orbitSpeed) * linearVelocity;

            var emission = ps.emission;
            emission.rateOverTimeMultiplier = emissionRateProp * linearVelocity;

            float baseZoom = zoomSizeFactor;
            float currentZoom = baseZoom + linearVelocity * zoomSpeedFactor;

            cometCamDistance = currentZoom;

            cometCam.transform.localEulerAngles = Camera.main.transform.localEulerAngles;

            if (Input.GetKeyUp(KeyCode.Space)) {
                DoLaunch();
            }
        } else if(currentState == State.Launched) {
            cometCam.Follow = transform;
            if (Input.GetMouseButtonDown(0)) {
                Wish(Camera.main.ScreenToWorldPoint(Input.mousePosition - Vector3.forward * (Camera.main.transform.position.z)));
            }
            float currentZoom = Mathf.Max(cometCamDistance,launchedZoom);

            cometCamDistance = currentZoom;
        } else if (currentState == State.Idle) {
            cometCamDistance = 160;

            cometCam.Follow = transform;
            transform.position = startPos.position;
            transform.localEulerAngles = startPos.localEulerAngles;
            rb.bodyType = RigidbodyType2D.Kinematic;
            var m = ps.main;
            m.simulationSpace = ParticleSystemSimulationSpace.World;
            if (Input.GetKeyDown(KeyCode.Space)) {
                lastVelocity = cometCam.transform.up * launchVelocity;
                DoLaunch();
            }
        } else {
            rb.bodyType = RigidbodyType2D.Dynamic;
            var m = ps.main;
            m.simulationSpace = ParticleSystemSimulationSpace.World;
        }
    }

    private Vector2 lastPosition;
    private Vector2 lastVelocity;
    private void FixedUpdate() {
        Vector2 currentPosition = rb.position;
        lastVelocity = (currentPosition - lastPosition)/Time.fixedDeltaTime;
        lastPosition = currentPosition;

        if (currentState == State.Launched) {
            Vector2 direction = massiveBody.transform.position - transform.position;
            direction.Normalize();

            rb.velocity += direction * massiveGravity * Time.fixedDeltaTime;
        }
        cometCam.transform.LookAt(cometCam.transform.position+Vector3.forward, (cometCam.transform.position.normalized - massiveBody.transform.position).normalized);
    }

    public void DoLand(OrbitBody landOn) {
        landOn.rr.gameObject.SetActive(false);

        speedProgress = 0.0f;

        Vector2 hitVelocity = rb.velocity;

        Vector2 bodyToComet = rb.position - (Vector2)landOn.transform.position;
        float angle = Vector2.SignedAngle(Vector2.up, bodyToComet.normalized);
        orbiter.angle = angle;

        Vector2 cometToBody = -bodyToComet;
        float hitAngle = Vector2.SignedAngle(cometToBody.normalized, hitVelocity.normalized);
        bool hitClockwise = hitAngle > 0.0f;


        orbiter.orbitSpeed = Mathf.Abs(orbiter.orbitSpeed) * (hitClockwise ? -1.0f : 1.0f);
        
        currentState = State.InOrbit;
        orbiter.objectToOrbit = landOn;
        rb.isKinematic = true;
        
        var m = ps.main;
        m.customSimulationSpace = landOn.transform;
        m.simulationSpace = ParticleSystemSimulationSpace.Custom;
        ps.Clear();

        OnLand.Invoke();
    }

    private void DoLaunch() {
        if (orbiter.objectToOrbit != null) {
            orbiter.objectToOrbit.rr.gameObject.SetActive(true);

            Cinemachine.CinemachineFramingTransposer t = cometCam.GetCinemachineComponent<Cinemachine.CinemachineFramingTransposer>();
        }

        currentState = State.Launched;
        orbiter.objectToOrbit = null;
        rb.isKinematic = false;
        rb.velocity = lastVelocity;
        OnLaunch.Invoke();
    }

    private void Wish(Vector2 point) {
        if(tracker.currentNumberOfWishes == 0) {
            return;
        }
        float mag = wishVelocity;
        Vector2 newDirection = point - rb.position;
        rb.velocity = newDirection.normalized * mag;
        tracker.UseWish();
        OnWish.Invoke();
    }
}
