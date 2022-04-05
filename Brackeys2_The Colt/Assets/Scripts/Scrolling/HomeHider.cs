using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeHider : MonoBehaviour
{
    private void Awake() {
        gameObject.SetActive(RunCountTracker.Instance.canGoHome);
    }
}
