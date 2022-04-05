using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CalendarMonthSpawner : MonoBehaviour
{
    public CalendarMonthController controllerPrefab;
    public Transform container;

    public TimeTracker tracker;
    public EventCalendar calendar;

    public int lookaheadDays;

    private DateTime spawnedUntil;

    private void Start() {
        spawnedUntil = tracker.startTime;
    }

    private void Update() {
        EnsureSpawn();
    }

    public void EnsureSpawn() {
        while (spawnedUntil <= tracker.currentTime.AddDays(lookaheadDays)) {
            SpawnNewMonth(new DateTime(spawnedUntil.Year, spawnedUntil.Month, 1));
            spawnedUntil = spawnedUntil.AddMonths(1);
        }
    }

    private void SpawnNewMonth(DateTime monthStart) {
        CalendarMonthController newController = Instantiate(controllerPrefab.gameObject, container).GetComponent<CalendarMonthController>();
        newController.timeTracker = tracker;
        newController.calendar = calendar;
        newController.monthStart = monthStart;
    }
}
