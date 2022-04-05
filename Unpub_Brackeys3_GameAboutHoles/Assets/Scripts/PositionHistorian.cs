using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionHistorian : Historian
{
    public PositionHistorian(MonoBehaviour o) : base(o) {
    }

    public struct PushBodyState {
        public Vector3 position;
    }

    public override object PrepareState() {
        PushBodyState newState = new PushBodyState();
        newState.position = objectToTrack.transform.position;
        return newState;
    }

    public override void SetState(object state, float transitionTime) {
        PushBodyState s = (PushBodyState)state;
        objectToTrack.StartCoroutine(MoveToPos(s.position, transitionTime));
    }

    private IEnumerator MoveToPos(Vector3 pos, float time) {
        foreach (Collider c in objectToTrack.GetComponentsInChildren<Collider>()) {
            c.enabled = false;
        }
        bool wasKinematic = objectToTrack.GetComponent<Rigidbody>().isKinematic;
        objectToTrack.GetComponent<Rigidbody>().isKinematic = true;
        float progress = 0.0f;
        Vector3 startPos = objectToTrack.transform.position;

        while (progress < 1.0f) {
            objectToTrack.transform.position = new Vector3(Mathf.SmoothStep(startPos.x, pos.x, progress),
                Mathf.SmoothStep(startPos.y, pos.y, progress),
                Mathf.SmoothStep(startPos.z, pos.z, progress));
            progress += Time.deltaTime / time;
            yield return null;
        }
        foreach (Collider c in objectToTrack.GetComponentsInChildren<Collider>()) {
            c.enabled = true;
        }
        objectToTrack.GetComponent<Rigidbody>().isKinematic = wasKinematic;
        isTransitioning = false;
    }
}
