using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelButton : MonoBehaviour {
    public LevelLoader loader;

    public string levelToLoad { get; set; }

    public void GoToNextLevel() {
        loader.LoadLevel(levelToLoad);
    }
}
