using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WishTrackerDisplay : MonoBehaviour
{
    public WishTracker tracker;
    public TMPro.TextMeshProUGUI field;

    // Update is called once per frame
    void Update()
    {
        field.text = tracker.currentNumberOfWishes.ToString();
    }
}
