using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TransitionTimeViewer : MonoBehaviour {
    public GameStateTransitionTimer timer;
    private TextMeshProUGUI textBox;

    private void Awake() {
        textBox = GetComponent<TextMeshProUGUI>();    
    }

    private void Update() {
        if(timer != null) {
            textBox.text = timer.timeLeft.ToString();
        }
    }
}
