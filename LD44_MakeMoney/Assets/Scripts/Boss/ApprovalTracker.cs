using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ApprovalTracker : MonoBehaviour
{
    public float maxApproval;

    public float currentApproval;

    public UnityEvent OnFired;

    public void SetApproval(float amount) {
        currentApproval = Mathf.Clamp(amount, 0.0f, maxApproval);
        if(currentApproval == 0) {
            OnFired.Invoke();
        }
    }

    public void GainApproval(float amount) {
        SetApproval(currentApproval + amount);
    }

    public void LoseApproval(float amount) {
        SetApproval(currentApproval - amount);
    }
}
