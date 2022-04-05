using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChoiceItem : MonoBehaviour {
    public string choiceName;
    public string choiceDescription;

    public TextMeshProUGUI nameField;
    public TextMeshProUGUI descriptionField;

    private void Start() {
        nameField.text = choiceName;
        descriptionField.text = choiceDescription;
    }
}
