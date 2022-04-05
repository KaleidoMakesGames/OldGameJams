using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenMovementConsumer : MonoBehaviour
{
    public MovementController mc;
    public float oxygenPerTile;

    public OxygenTracker tracker;

    private Vector3 lastPosition;

    private void Start() {
        lastPosition = mc.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(mc.isMovingToTile) {
            Vector2 delta = mc.transform.position - lastPosition;
            tracker.currentOxygen -= delta.magnitude * oxygenPerTile;
        }
        lastPosition = mc.transform.position;
        
        mc.enabled = tracker.currentOxygen >= oxygenPerTile-0.01;
    }
}
