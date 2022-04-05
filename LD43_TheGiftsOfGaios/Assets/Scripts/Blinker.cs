using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinker : MonoBehaviour {
    public float blinkFrequency = 0.0f;

    public Transform objectToBlink;
    
    private void OnEnable() {
        StartCoroutine(DoBlink());    
    }

    IEnumerator DoBlink() {
        while(true) {
            objectToBlink.gameObject.SetActive(!objectToBlink.gameObject.activeSelf);
            if(blinkFrequency == 0.0f) {
                break;
            }
            yield return new WaitForSecondsRealtime(1 / blinkFrequency);
        }
    }
}
