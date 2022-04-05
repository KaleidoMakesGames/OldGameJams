using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EventPool : MonoBehaviour {
    public TimeTracker tracker;

    public struct PoolEvent {
        public float probabilityOfEvent;
        public DateTime activationTime;
        public Event eventInfo;
    }

    public List<PoolEvent> eventsInPool;

    public EventExecutor executor;

    private DateTime lastExecution;

    private void Awake() {
        eventsInPool = new List<PoolEvent>();
    }

    private void Update() {
        if ((tracker.currentTime - lastExecution).TotalDays >= 1) {
            UpdatePool();
            lastExecution = tracker.currentTime;
        }
    }

    public void Add(Event.PoolOutcome outcome) {
        PoolEvent newEvent;
        newEvent.activationTime = Event.DateInfo.GetDateTime(outcome.startDate, tracker.currentTime, tracker.startTime);
        newEvent.probabilityOfEvent = outcome.expectedTimesPerYear * (1.0f/365.0f);
        newEvent.eventInfo = outcome.eventToAdd;
        eventsInPool.Add(newEvent);
    }

    public void RemoveType(Event type) {
        eventsInPool.RemoveAll(x => x.eventInfo == type);
    }

    private void UpdatePool() {
        List<PoolEvent> eventsToExecute = new List<PoolEvent>();
        List<PoolEvent> newPool = new List<PoolEvent>();

        foreach(PoolEvent eventInPool in eventsInPool) {
            if (eventInPool.activationTime <= tracker.currentTime && UnityEngine.Random.Range(0.0f, 1.0f) <= eventInPool.probabilityOfEvent) {
                eventsToExecute.Add(eventInPool);
            } else {
                newPool.Add(eventInPool);
            }
        }

        eventsInPool = newPool;
        foreach(PoolEvent eventToExecute in eventsToExecute) {
            executor.Execute(eventToExecute.eventInfo);
        }
    }
}
