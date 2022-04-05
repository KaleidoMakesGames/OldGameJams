using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBCLogic : MonoBehaviour
{
    public NavigationAgent navAgent;
    public OxygenTracker oxygenTracker;

    public enum State {
        RefillingAtLung,
        Delivering
    }
    public State currentState;

    // Update is called once per frame
    void Update()
    {
        switch(currentState) {
            case State.RefillingAtLung:
                OxygenTracker closestLung = null;
                float highestPriorityLungValue = Mathf.Infinity;
                foreach (OxygenTracker tracker in OxygenTracker.allTrackers) {
                    if (tracker.oxygenPriority < oxygenTracker.oxygenPriority) {
                        float distance = Vector2Int.Distance(oxygenTracker.gridElement.position, tracker.gridElement.position);
                        float percentageFull = tracker.currentOxygen / tracker.maxOxygen;
                        float lungPriority = percentageFull / (distance/10 + 0.1f);
                        if(percentageFull < 0.1f) {
                            lungPriority = Mathf.NegativeInfinity;
                        }
                        if (lungPriority < highestPriorityLungValue) {
                            highestPriorityLungValue = distance;
                            closestLung = tracker;
                        }
                    }
                }
                if (closestLung != null) {
                    navAgent.destination = closestLung.gridElement.position;
                }
                if (oxygenTracker.currentOxygen == oxygenTracker.maxOxygen) {
                    currentState = State.Delivering;
                }
                break;
            case State.Delivering:
                if(oxygenTracker.currentOxygen == 0) {
                    currentState = State.RefillingAtLung;
                }

                OxygenTracker highestPriority = null;
                float highestPriorityValue = Mathf.NegativeInfinity;
                foreach (OxygenTracker tracker in OxygenTracker.allTrackers) {
                    if (tracker.oxygenPriority > oxygenTracker.oxygenPriority) {
                        float distance = Vector2Int.Distance(oxygenTracker.gridElement.position, tracker.gridElement.position);
                        float percentageFull = tracker.currentOxygen / tracker.maxOxygen;
                        float priority = 1 / ((percentageFull+0.1f) * (distance/10.0f+0.1f));
                        if(percentageFull == 1) {
                            priority = Mathf.NegativeInfinity;
                        }
                        if (priority > highestPriorityValue) {
                            highestPriorityValue = priority;
                            highestPriority = tracker;
                        }
                    }
                }
                if (highestPriority != null) {
                    navAgent.destination = highestPriority.gridElement.position;
                }
                break;
        }
    }
}
