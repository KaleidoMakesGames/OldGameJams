using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class OptionDisplayer : MonoBehaviour
{
    public UnityEvent OnOptionSelected;
    public Event.Option optionInfo;
    public float? optionPaymentAmount = null;

    public TextMeshProUGUI nameField;
    public TextMeshProUGUI paymentField;

    public void SelectOption() {
        OnOptionSelected.Invoke();
    }

    private void Update() {
        nameField.text = optionInfo.optionName;
        if (optionPaymentAmount.HasValue) {
            paymentField.gameObject.SetActive(true);
            paymentField.text = "" + (optionPaymentAmount.Value > 0 ? "+" : "-") + Mathf.Abs(optionPaymentAmount.Value).ToString("C2") + "";
        } else {
            paymentField.gameObject.SetActive(false);
        }
    }
}
