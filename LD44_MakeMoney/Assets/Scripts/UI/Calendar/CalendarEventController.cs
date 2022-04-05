using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CalendarEventController : MonoBehaviour
{
    public TextMeshProUGUI dateText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI amountText;

    public EventCalendar.ScheduledEvent scheduledEvent;

    private void Start() {
        UpdateEvent();
    }

    private void Update() {
        UpdateEvent();
    }

    // Update is called once per frame
    void UpdateEvent()
    {
        if(scheduledEvent.eventInfo != null) {
            dateText.text = scheduledEvent.dateOfEvent.ToString("MM/dd/yyyy");
            nameText.text = scheduledEvent.eventInfo.eventName;

            if (scheduledEvent.eventInfo.options.Count == 1 && scheduledEvent.eventInfo.options[0].payment != null) {
                amountText.gameObject.SetActive(true);
                amountText.text = scheduledEvent.eventInfo.options[0].payment.GetAmountText();
            } else {
                amountText.gameObject.SetActive(false);
            }
        }
    }
}
