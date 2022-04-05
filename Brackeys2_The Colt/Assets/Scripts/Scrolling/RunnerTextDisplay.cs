using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RunnerTextDisplay : MonoBehaviour
{
    public TextMeshProUGUI textField;
    public RunnerDistanceTracker tracker;

    // Update is called once per frame
    void Update()
    {
        textField.text = string.Format("<b>You made it {0:0} yards from home.</b>\nThe stableman will help you recover.\n\n Her town was probably just a little further...", tracker.distanceTraveled);
    }
}
