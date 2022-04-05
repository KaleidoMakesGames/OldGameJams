using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CalendarDisplayer : MonoBehaviour
{
    public TextMeshProUGUI textField;
    public EventCalendar calendar;

    private void Update() {
        textField.text = GetCalendarText();
    }

    private string GetCalendarText() {
        string text = "";
        foreach(var sEvent in calendar.scheduledEvents) {
            string eventText = sEvent.dateOfEvent.ToString("MM/dd/yy") + "\n";
            eventText += "\t<b>" + sEvent.eventInfo.name + "</b>\n";
            eventText += "\t" + sEvent.eventInfo.eventDescription + "\n";
            eventText += "\t" + sEvent.eventInfo.options[0].payment.GetAmountText() + "\n";
            text += eventText;
        }
        return text;
    }
}
