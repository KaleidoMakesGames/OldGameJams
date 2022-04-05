using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreenMaker : MonoBehaviour
{
    public void Toggle() {
        bool newFS = !Screen.fullScreen;
        Screen.fullScreenMode = newFS ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        Screen.fullScreen = newFS;
        Debug.Log("Fullscreen: " + Screen.fullScreen);
    }
}
