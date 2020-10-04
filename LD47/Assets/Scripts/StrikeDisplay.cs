using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrikeDisplay : MonoBehaviour
{
    public TMPro.TextMeshProUGUI textField;
    public StrikesCounter strikeCounter;

    private void Update() {
        textField.text = strikeCounter.strikes.ToString() + "/" + strikeCounter.untilLoss.ToString();
    }
}