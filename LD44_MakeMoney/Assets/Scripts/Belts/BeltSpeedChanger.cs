using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BeltSpeedChanger : MonoBehaviour
{
    public bool isUp;
    public BeltController beltToControl;

    public UnityEvent OnClick;

    public void DoChange() {
        if (isUp) {
            beltToControl.SpeedUp();
        } else {
            beltToControl.SlowDown();
        }
        OnClick.Invoke();
    }
}
