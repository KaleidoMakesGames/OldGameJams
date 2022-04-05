using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackFinishListener : MonoBehaviour {
    public UnityEvent OnAttackFinished;

    public void AttackFinished() {
        OnAttackFinished.Invoke();
    }
}
