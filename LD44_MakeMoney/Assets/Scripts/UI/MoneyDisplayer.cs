using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
[ExecuteInEditMode]
public class MoneyDisplayer : MonoBehaviour
{
    public MoneyTracker tracker;
    private TextMeshProUGUI textField;

    private void Awake() {
        textField = GetComponent<TextMeshProUGUI>();    
    }

    // Update is called once per frame
    void Update()
    {
        if(tracker != null) {
            if (tracker.currentMoney < 0.0f) {
                textField.text = "BROKE";
            } else {
                textField.text = tracker.currentMoney.ToString("C2");
            }
        }    
    }
}
