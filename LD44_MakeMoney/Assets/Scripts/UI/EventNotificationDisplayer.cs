using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventNotificationDisplayer : MonoBehaviour
{
    public Event eventInfo;
    public EventExecutor executor;

    public LayoutElement selfLayout;

    public OptionDisplayer optionDisplayerPrefab;

    public Transform optionContainer;

    public TextMeshProUGUI nameField;
    public TextMeshProUGUI descriptionField;
    public TextMeshProUGUI dateField;
    public TextMeshProUGUI paymentField;

    public Image notificationColor;

    public DateTime timeOfEvent;

    public float realAmount;

    public float lifetime;

    public LayoutGroup containerLayout;

    private OptionDisplayer defaultDisplayer;

    private void Start() {

        if (!eventInfo.doFirstOption) {
            lifetime = Mathf.Infinity;
            optionContainer.gameObject.SetActive(true);
            paymentField.gameObject.SetActive(false);
            foreach (Event.Option option in eventInfo.options) {
                float? payment = null;
                if(option.payment != null) {
                    payment = (option.payment.isCharge ? -1.0f : 1.0f) * option.payment.GetAmount(executor.moneyTracker);
                }

                OptionDisplayer newDisplayer = Instantiate(optionDisplayerPrefab.gameObject, optionContainer).GetComponent<OptionDisplayer>();
                newDisplayer.optionInfo = option;
                newDisplayer.optionPaymentAmount = payment;
                newDisplayer.OnOptionSelected.AddListener(delegate {
                    executor.ExecuteOption(eventInfo, option, payment, false);
                    executor.PlaySelectSound();
                    defaultDisplayer = null;
                    Destroy(gameObject);
                });

                if(defaultDisplayer == null) {
                    defaultDisplayer = newDisplayer;
                }
            }
        } else {
            paymentField.gameObject.SetActive(true);
        }


        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());

        UpdateText();

        StartCoroutine(DoDestroy());
    }

    private void Update() {
        UpdateText();
    }

    private void UpdateText() {
        if(eventInfo != null) {
            nameField.text = eventInfo.eventName;
            descriptionField.text = eventInfo.eventDescription;
            dateField.text = timeOfEvent.ToString("MM/dd/yyyy");
            paymentField.text = (realAmount > 0 ? "+" : "-") + Mathf.Abs(realAmount).ToString("C2");
        }

        selfLayout.preferredHeight = containerLayout.preferredHeight;

        notificationColor.color = eventInfo.type.color;
    }

    public IEnumerator DoDestroy() {
        yield return new WaitForSeconds(lifetime);
        if(defaultDisplayer != null) {
            defaultDisplayer.SelectOption();
        }
        Destroy(gameObject);
    }
}
