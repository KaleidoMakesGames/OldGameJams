using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class FadeFinishListener : MonoBehaviour
{
    public UnityEvent OnFadedOut;
    public UnityEvent OnFadedIn;

    private Animator animator;
    
    public void SetFaded(bool faded) {
        gameObject.SetActive(true);
        animator = GetComponent<Animator>();
        animator.SetBool("ShouldBeFaded", faded);
    }

    public void FadedOut() {
        OnFadedOut.Invoke();
    }

    public void FadedIn() {
        OnFadedIn.Invoke();
    }
}
