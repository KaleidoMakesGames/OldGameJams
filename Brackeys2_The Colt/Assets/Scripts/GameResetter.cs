using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResetter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RunCountTracker.Instance.numberOfRuns = 0;   
    }
}
