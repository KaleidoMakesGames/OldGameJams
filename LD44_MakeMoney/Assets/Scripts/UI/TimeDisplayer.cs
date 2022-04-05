using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeDisplayer : MonoBehaviour {
    public TimeTracker tracker;

    public TextMeshProUGUI dateField;
    public TextMeshProUGUI ageField;

    // Update is called once per frame
    void Update()
    {
        dateField.text = tracker.currentTime.ToString("MM/dd/yyyy");
        ageField.text = string.Format("You are {0} years old", tracker.currentAge);
    }
}
