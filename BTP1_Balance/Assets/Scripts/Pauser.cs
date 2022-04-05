using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pauser : MonoBehaviour {
    public void DoPause() {
        Time.timeScale = 0.0f;
    }

    public void DoResume() {
        Time.timeScale = 1.0f;
    }
}
