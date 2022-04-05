using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunCountTracker : Singleton<RunCountTracker>
{
    [HideInInspector] public int numberOfRuns;
    public int homeThreshold;

    public bool canGoHome {
        get {
            return numberOfRuns > homeThreshold - 1;
        }
    }
}
