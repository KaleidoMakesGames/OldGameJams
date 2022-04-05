using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CalendarMonthController : MonoBehaviour
{
    public TimeTracker timeTracker;
    public EventCalendar calendar;

    public DateTime monthStart;

    public TextMeshProUGUI monthNameField;

    public float unitsPerDay;

    public CalendarEventController eventControllerPrefab;

    public Transform container;

    public List<EventCalendar.ScheduledEvent> eventsInMonth;

    public List<CalendarEventController> controllers;

    private RectTransform thisTransform;

    private void Awake() {
        thisTransform = GetComponent<RectTransform>();
    }

    private void Start() {
        int daysInMonth = DateTime.DaysInMonth(monthStart.Year, monthStart.Month);
        thisTransform.sizeDelta = new Vector2(unitsPerDay * daysInMonth, thisTransform.sizeDelta.y);
        UpdateController();
    }

    private void Update() {
        UpdateController();
    }

    private void UpdateController() {
        monthNameField.text = monthStart.ToString("MMMM yyyy");

        thisTransform.anchoredPosition = new Vector2(CalculatePosition(), thisTransform.anchoredPosition.y);

        if(thisTransform.anchoredPosition.x < - thisTransform.sizeDelta.x) {
            Destroy(gameObject);
        }

        eventsInMonth = calendar.GetEventsInMonth(monthStart);
        EnsureEventsInMonth();
        for(int i = 0; i < eventsInMonth.Count; i++) {
            controllers[i].scheduledEvent = eventsInMonth[i];
        }
    }

    private void EnsureEventsInMonth() {
        if(controllers.Count > eventsInMonth.Count) {
            int extra = controllers.Count - eventsInMonth.Count;
            for(int i = 0; i < extra; i++) {
                int endIndex = controllers.Count-1;
                Destroy(controllers[endIndex].gameObject);
                controllers.RemoveAt(endIndex);
            }
        }

        if(controllers.Count < eventsInMonth.Count) {
            int extra = eventsInMonth.Count - controllers.Count;
            for (int i = 0; i < extra; i++) {
                controllers.Add(Instantiate(eventControllerPrefab.gameObject, container).GetComponent<CalendarEventController>());
            }
        }
    }

    private float CalculatePosition() {
        DateTime currentTime = timeTracker.currentTime;
        float daysUntilZero = (float)((monthStart - currentTime).TotalDays);
        return daysUntilZero * unitsPerDay;
    }
}
