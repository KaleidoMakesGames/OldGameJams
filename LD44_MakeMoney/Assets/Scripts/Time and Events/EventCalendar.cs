using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EventCalendar : MonoBehaviour
{
    public struct ScheduledEvent {
        public DateTime dateOfEvent;
        public Event eventInfo;
    }
    
    public List<ScheduledEvent> scheduledEvents { get; set; }

    public TimeTracker tracker;

    public EventExecutor executor;

    private void Awake() {
        scheduledEvents = new List<ScheduledEvent>();
    }

    public void Add(Event.CalendarOutcome outcome) {
        ScheduledEvent newEvent;
        newEvent.dateOfEvent = Event.DateInfo.GetDateTime(outcome.dateInfo, tracker.currentTime, tracker.startTime);
        newEvent.eventInfo = outcome.eventToAdd;
        scheduledEvents.Add(newEvent);

        scheduledEvents.Sort((x, y) => DateTime.Compare(x.dateOfEvent, y.dateOfEvent));
    }

    public void RemoveType(Event type) {
        scheduledEvents.RemoveAll(x => x.eventInfo == type);
    }

    private void Update() {
        List<ScheduledEvent> eventsToExecute = new List<ScheduledEvent>();
        List<ScheduledEvent> eventsRemaining = new List<ScheduledEvent>();

        foreach(ScheduledEvent scheduledEvent in scheduledEvents) { 
            if(scheduledEvent.dateOfEvent <= tracker.currentTime) {
                eventsToExecute.Add(scheduledEvent);
            } else {
                eventsRemaining.Add(scheduledEvent);
            }
        }

        scheduledEvents = eventsRemaining;
        foreach(ScheduledEvent eventToExecute in eventsToExecute) {
            executor.Execute(eventToExecute.eventInfo);
        }
    }

    public List<ScheduledEvent> GetEventsInMonth(DateTime monthStart) {
        List<ScheduledEvent> eventsInMonth = new List<ScheduledEvent>();
        foreach(ScheduledEvent scheduledEvent in scheduledEvents) {
            if(scheduledEvent.eventInfo.planned && scheduledEvent.dateOfEvent.Year == monthStart.Year && scheduledEvent.dateOfEvent.Month == monthStart.Month) {
                eventsInMonth.Add(scheduledEvent);
            }
        }
        return eventsInMonth;
    }
}
