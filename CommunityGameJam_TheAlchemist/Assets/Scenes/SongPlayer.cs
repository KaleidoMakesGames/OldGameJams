using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongPlayer : MonoBehaviour
{
    public static SongPlayer globalPlayer;
    private void Awake() {
        if (globalPlayer == null) {
            DontDestroyOnLoad(gameObject);
            globalPlayer = this;
        } else {
            Destroy(gameObject);
        }
    }
}
