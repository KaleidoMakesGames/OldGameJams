using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveUpController : MonoBehaviour
{
    public float timeout;

    public GameObject text;
    public GameObject arrow;
    
    public OxygenMovementConsumer c;

    private bool hasTimedOut = false;

    private bool counterStarted = false;
    private float counterStartTime;
    // Update is called once per frame
    void Update()
    {
        text.SetActive(hasTimedOut);
        arrow.SetActive(hasTimedOut);
        
        if (c.tracker.currentOxygen <= c.oxygenPerTile - 0.01f) {
            if(!counterStarted) {
                counterStarted = true;
                counterStartTime = Time.time;
            } else {
                if(Time.time - counterStartTime >= timeout) {
                    hasTimedOut = true;
                }
            }
        } else {
            hasTimedOut = false;
            counterStarted = false;
        }
    }
}
