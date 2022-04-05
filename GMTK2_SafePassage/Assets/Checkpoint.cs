using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static Checkpoint lastReached = null;
    
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.isTrigger) {
            return;
        }
        CarController c = collision.GetComponentInParent<CarController>();
        if(c != null) {
            lastReached = this;

            c.OnCheckpointReached();
            GetComponent<Collider2D>().enabled = false;
        }
    }

    public void ResetToCheckpoint(CarController c) {
        c.transform.position = transform.position;
        c.transform.eulerAngles = Vector3.up * 180.0f * (Mathf.Sign(transform.position.x) > 0 ? 1 : 0);

        var l = LevelConnection.GetSortedConnections();

        // Find closest connection
        float dist = Mathf.Infinity;
        int closestIndex = -1;
        for(int i = 0; i < l.Count; i++) {
            float d = Vector2.Distance(l[i].GetPointAlongCurve(1.0f), transform.position);
            if(d < dist) {
                closestIndex = i;
                dist = d;
            }
        }

        float distToStart = Mathf.Abs(transform.position.y - l[closestIndex].GetPointAlongCurve(0).y);
        float distToEnd = Mathf.Abs(transform.position.y - l[closestIndex].GetPointAlongCurve(1).y);

        bool hasPassedClose = distToEnd < distToStart;


        for (int i = 0; i < l.Count; i++) {
            l[i].enabled = false;
        }

        if (hasPassedClose) {
            l[closestIndex].UpdateAllColliders();
            l[closestIndex + 1].enabled = true;
        } else {
            l[closestIndex - 1].UpdateAllColliders();
            l[closestIndex].enabled = true;
        }

        FindObjectOfType<LevelTargeter>().Advance();
        c.ResetToCheckpoint();
    }

    public static void ResetToLastCheckpoint(CarController c) {
        c.rb.velocity = Vector3.zero;
        c.rb.angularVelocity = 0.0f;
        c.isRunning = false;
        if (lastReached == null) {
            c.FullReset();
            return;
        }
        lastReached.ResetToCheckpoint(c);
    }
}
