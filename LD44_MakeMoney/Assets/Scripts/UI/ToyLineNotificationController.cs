using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToyLineNotificationController : MonoBehaviour
{
    public string text;

    public bool isGood;

    public Color goodColor;
    public Color badColor;

    public TextMeshProUGUI textField;

    private void Start() {
        textField.text = text;
        textField.color = isGood ? goodColor : badColor;
    }

    public void FinishedMoveUp() {
        Destroy(gameObject);
    }
}
