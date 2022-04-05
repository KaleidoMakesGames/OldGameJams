using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventExecutor : MonoBehaviour
{
    public EventPool pool;
    public EventCalendar calendar;
    public MoneyTracker moneyTracker;
    public TimeTracker timeTracker;

    [System.Serializable]
    public class EventEvent : UnityEvent<Event, float, DateTime> { }
    public UnityEvent OnPlaySelectSound;
    public EventEvent OnEventExecuted;

    public Event creationEvent;

    public void Execute(Event eventInfo) {
        if(eventInfo == null) {
            return;
        }
        if(eventInfo.options.Count == 0) {
            return;
        } else if(eventInfo.options.Count == 1 && eventInfo.doFirstOption) {
            ExecuteOption(eventInfo, eventInfo.options[0]);
        } else {
            OnEventExecuted.Invoke(eventInfo, 0.0f, timeTracker.currentTime);
        }
    }

    public void PlaySelectSound() {
        OnPlaySelectSound.Invoke();
    }


    public void ExecuteOption(Event eventInfo, Event.Option option, float? paymentOverride = null, bool notify = true) {
        if(option.payment != null || paymentOverride.HasValue) {
            float amount = paymentOverride.HasValue ? Mathf.Abs(paymentOverride.Value) : option.payment.GetAmount(moneyTracker);
            bool isCharge = paymentOverride.HasValue ? (paymentOverride.Value < 0) : option.payment.isCharge;
            if (isCharge) {
                moneyTracker.SpendMoney(amount);
                if (notify) {
                    OnEventExecuted.Invoke(eventInfo, -amount, timeTracker.currentTime);
                }
            } else {
                moneyTracker.GainMoney(amount);
                if (notify) {
                    OnEventExecuted.Invoke(eventInfo, amount, timeTracker.currentTime);
                }
            }
        }
        
        foreach(Event.CalendarOutcome calendarOutcome in option.eventsToAddToCalendar) {
            calendar.Add(calendarOutcome);
        }

        foreach(Event.PoolOutcome poolOutcome in option.eventsToAddToPool) {
            pool.Add(poolOutcome);
        }

        foreach(Event eventType in option.eventTypesToRemoveFromCalendar) {
            calendar.RemoveType(eventType);
        }

        foreach (Event eventType in option.eventTypesToRemoveFromPool) {
            pool.RemoveType(eventType);
        }
    }
}
