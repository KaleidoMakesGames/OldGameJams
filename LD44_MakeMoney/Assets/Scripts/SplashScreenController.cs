using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SplashScreenController : MonoBehaviour {

    public float screenTime;
    public UnityEvent OnSplashScreenFinished;

    private void Start() {
        StartCoroutine(WaitForScreenTime());    
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKey) {
            OnSplashScreenFinished.Invoke();
        }
    }

    private IEnumerator WaitForScreenTime() {
        yield return new WaitForSeconds(screenTime);
        OnSplashScreenFinished.Invoke();
    }
}
