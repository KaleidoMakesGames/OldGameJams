using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TimeTracker : MonoBehaviour
{
    public DateTime currentTime { get; private set; }
    public DateTime startTime { get; private set; }

    public int currentAge {
        get {
            int age = currentTime.Year - birthday.Year;
            if(currentTime < birthday.AddYears(age)) {
                age--;
            }
            return age;
        }
    }

    public int startAge;

    public float fakeDaysPerRealSecond;

    private DateTime birthday;

    private void Awake() {
        startTime = DateTime.Now;
        currentTime = startTime;
        birthday = currentTime.AddYears(-startAge);
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = currentTime.AddDays(fakeDaysPerRealSecond * Time.deltaTime);
    }
}
