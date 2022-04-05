using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName ="Make Money/Event")]
public class Event : ScriptableObject
{
    public string eventName;
    public bool planned;
    public string eventDescription;
    public EventType type;

    public bool doFirstOption = true;
    
    [System.Serializable]
    public struct Option {
        public string optionName;
        [Header("Outcome")]
        public Payment payment;
        public List<Event> eventTypesToRemoveFromCalendar;
        public List<Event> eventTypesToRemoveFromPool;
        public List<CalendarOutcome> eventsToAddToCalendar;
        public List<PoolOutcome> eventsToAddToPool;
    }

    [System.Serializable]
    public struct CalendarOutcome {
        public Event eventToAdd;

        public DateInfo dateInfo;
    }

    [System.Serializable]
    public struct DateInfo {
        [Header("Year")]
        public int yearNumber;
        public bool relativeYear;

        [Header("Month")]
        public int monthNumber;
        public bool relativeMonth;

        [Header("Day")]
        public int dayNumber;
        public bool relativeDay;

        public static DateTime GetDateTime(DateInfo info, DateTime currentTime, DateTime startTime) {
            DateTime newTime = info.relativeYear ? currentTime.AddYears(info.yearNumber) : startTime.AddYears(info.yearNumber);
            newTime = info.relativeMonth ? newTime.AddMonths(info.monthNumber) : new DateTime(newTime.Year, info.monthNumber, newTime.Day);
            newTime = info.relativeDay ? newTime.AddDays(info.dayNumber) : new DateTime(newTime.Year, newTime.Month, info.dayNumber);
            if(newTime < currentTime) {
                newTime.AddYears(1);
            }
            return newTime;
        }
    }

    [System.Serializable]
    public struct PoolOutcome {
        public Event eventToAdd;
        public float expectedTimesPerYear;
        public DateInfo startDate;
    }

    public List<Option> options;
}
