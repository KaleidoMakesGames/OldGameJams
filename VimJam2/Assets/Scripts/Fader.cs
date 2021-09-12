using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    public Image faderImage;
    public AnimationCurve fadeCurve = AnimationCurve.Linear(0, 0, 1, 1);
    public float fadeTime;

    public bool isAnimating { get; private set; }

    public delegate void OnDoneDelegate();
    public void FadeIn(OnDoneDelegate onDoneDelegate = null) {
        StartCoroutine(DoFade(true, onDoneDelegate));
    }

    public void FadeOut(OnDoneDelegate onDoneDelegate = null) {
        StartCoroutine(DoFade(false, onDoneDelegate));
    }

    public IEnumerator DoFade(bool fadeIn, OnDoneDelegate onDoneDelegate) {
        if(isAnimating) {
            yield break;
        }
        isAnimating = true;
        float goalAlpha = fadeIn ? 0.0f : 1.0f;
        float startAlpha = 1 - goalAlpha;
        float startTime = Time.time;
        while(true) {
            float progress = (Time.time - startTime) / fadeTime;
            if(progress >= 1) {
                break;
            }
            faderImage.color = new Color(faderImage.color.r,
                faderImage.color.g,
                faderImage.color.b,
                Mathf.Lerp(startAlpha, goalAlpha, fadeCurve.Evaluate(progress)));
            yield return null;
        }

        isAnimating = false;
        if(onDoneDelegate != null) {
            onDoneDelegate();
        }
    }
}
