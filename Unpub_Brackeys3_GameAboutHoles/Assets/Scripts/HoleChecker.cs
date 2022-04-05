using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HoleChecker : MonoBehaviour
{
    public UnityEvent OnWin;
    [HideInInspector] public bool hasPlayed;
    public LevelLoader loader;

    public UnityEvent OnWinAllGame;

    private void Awake() {
        hasPlayed = false;
    }

    private void Update() {
        if (AllInHoles()) {
            if (!hasPlayed) {
                if (loader.currentLevel + 1 >= loader.levels.Count) {
                    OnWinAllGame.Invoke();
                } else {
                    OnWin.Invoke();
                    hasPlayed = true;
                }
            }
        }
    }

    private bool AllInHoles() { 
        // TODO: Inefficient
        foreach(HoleDetector h in FindObjectsOfType<HoleDetector>()) { 
            if(!h.isFilled) {
                return false;
            }
        }
        return true;
    }
}
