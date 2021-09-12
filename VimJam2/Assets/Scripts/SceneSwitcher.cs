using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public Fader fader;
    private void Start() {
        fader.FadeIn();
    }

    public void GoToScene(string sceneName) {
        fader.FadeOut(delegate {
            SceneManager.LoadScene(sceneName);
        });
    }
}
