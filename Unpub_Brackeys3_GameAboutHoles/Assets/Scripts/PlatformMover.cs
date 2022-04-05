using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlatformMover : MonoBehaviour
{
    public HistorianManager historianManager;
    public PlayArea area;

    public float distancePerUnit;
    public float moveSpeed;

    public AudioClip startMove;
    public AudioClip endMove;
    
    public AudioSource audioSource;

    public void MoveE() {
        StartCoroutine(DoMove(Vector3.right));
    }
    public void MoveN() {
        StartCoroutine(DoMove(Vector3.forward));
    }
    public void MoveS() {
        StartCoroutine(DoMove(Vector3.back));
    }
    public void MoveW() {
        StartCoroutine(DoMove(Vector3.left));
    }

    public UnityEvent OnMove;

    private Rigidbody _rb;

    [HideInInspector] public bool isMoving = false;

    private void Awake() {
        _rb = GetComponent<Rigidbody>();
    }

    private IEnumerator DoMove(Vector3 direction) {
        if(isMoving) {
            yield break;
        }
        
        Vector3 startPos = transform.position;
        Vector3 goalPos = startPos + direction * distancePerUnit;
        float moveTime = (goalPos - startPos).magnitude / moveSpeed;

        Bounds selfBounds = new Bounds();
        bool initialized = false;
        foreach(Collider c in GetComponentsInChildren<Collider>()) {
            if (initialized) {
                selfBounds.Encapsulate(c.bounds);
            } else {
                selfBounds = c.bounds;
                initialized = true;
            }
        }
        selfBounds.center += direction*distancePerUnit;
        Bounds areaBounds = area.bounds;
        Vector3 minPoint = new Vector3(selfBounds.min.x, areaBounds.center.y, selfBounds.min.z);
        Vector3 maxPoint = new Vector3(selfBounds.max.x, areaBounds.center.y, selfBounds.max.z);
        bool fullyContained = areaBounds.Contains(minPoint) && areaBounds.Contains(maxPoint);
        Debug.Log(areaBounds);
        if (!fullyContained) {
            yield break;
        }

        isMoving = true;

        audioSource.PlayOneShot(startMove);

        OnMove.Invoke();

        PushBody.FreezeAll();

        float progress = 0.0f;
        while(progress < 1.0f) {
            float x = Mathf.SmoothStep(startPos.x, goalPos.x, progress);
            float y = Mathf.SmoothStep(startPos.y, goalPos.y, progress);
            float z = Mathf.SmoothStep(startPos.z, goalPos.z, progress);
            _rb.MovePosition(new Vector3(x, y, z));
            progress += Time.deltaTime/moveTime;
            yield return null;
        }

        isMoving = false;

        audioSource.PlayOneShot(endMove);

        PushBody.ThawAll();

        historianManager.Record();
    }
}
