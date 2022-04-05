using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayMovementThreshold : MonoBehaviour {
    public float time;

    public UnityEvent OnMove;

    private bool isMoving;

    private Vector3 lastPosition;

    private void Start() {
        StartCoroutine(PlayFrequency());
    }

    private void Update() {
        isMoving = transform.position != lastPosition;
            
        lastPosition = transform.position;
    }

    private IEnumerator PlayFrequency() {
        while(true) {
            OnMove.Invoke();
            yield return new WaitForSeconds(time);
        }
    }
}
