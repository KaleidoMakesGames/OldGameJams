using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenRadiusDisplay : MonoBehaviour
{
    public OxygenTracker tracker;

    public SpriteRenderer spriteRenderer;

    private void Start() {
        Update();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.one * tracker.providingRadius * 2;
        spriteRenderer.enabled = tracker.currentOxygen > 0;
    }
}
