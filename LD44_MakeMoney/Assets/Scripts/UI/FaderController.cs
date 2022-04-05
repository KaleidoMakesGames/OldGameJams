using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class FaderController : MonoBehaviour
{
    public float fadeTime = 1.0f;
    public UnityEvent OnFadeInFinished;
    public UnityEvent OnFadeOutFinished;

    public bool startFadedIn = true;

    private Animator animator;
    public bool isFadedIn {
        get {
            return animator.GetBool("IsFadedIn");
        }
        set {
            animator.SetBool("IsFadedIn", value);
        }
    }

    private void Awake() {
        animator = GetComponent<Animator>();
        animator.speed = 1/fadeTime;
        GetComponent<Image>().enabled = true;
        isFadedIn = startFadedIn;
    }

    public void FinishFadeIn() {
        OnFadeInFinished.Invoke();
    }

    public void FinishFadeOut() {
        OnFadeOutFinished.Invoke();
    }

    private void OnValidate() {
        fadeTime = Mathf.Clamp(fadeTime, 0.001f, Mathf.Infinity);
    }
}
