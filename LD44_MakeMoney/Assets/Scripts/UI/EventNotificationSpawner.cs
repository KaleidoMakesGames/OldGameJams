using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EventNotificationSpawner : MonoBehaviour
{
    public EventNotificationDisplayer singleOptionDisplayerPrefab;

    public EventExecutor executor;

    public List<EventType> types;

    public void DisplayEvent(Event eventInfo, float realAmount, DateTime timeOfEvent) {
        if (types.Contains(eventInfo.type)) {
            EventNotificationDisplayer newDisplayer = Instantiate(singleOptionDisplayerPrefab.gameObject, transform).GetComponent<EventNotificationDisplayer>();
            newDisplayer.eventInfo = eventInfo;
            newDisplayer.executor = executor;
            newDisplayer.realAmount = realAmount;
            newDisplayer.timeOfEvent = timeOfEvent;
        }
    }
}
