using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeaveDetector : MonoBehaviour
{
    public UnityEvent OnLeave;
    private void OnTriggerExit(Collider other) {
        OnLeave.Invoke();
    }
}
