using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerDisplay : MonoBehaviour
{
    public Timer timer;
    public TMPro.TextMeshProUGUI textField;

    private void Update() {
        textField.text = timer.timeRemaining.ToString();
    }
}
