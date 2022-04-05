using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fader : MonoBehaviour
{
    public float fadeTime;
    public bool startOpaque = false;
    public bool isOpaque = false;
    public Image image;

    private void Start() {
        Color c = image.color;
        c.a = startOpaque ? 1.0f : 0.0f;
        image.color = c;
    }

    private void Update() {
        Color c = image.color;
        c.a = Mathf.MoveTowards(c.a, isOpaque ? 1.0f : 0.0f, fadeTime * Time.deltaTime);
        image.color = c;
    }

    public void FadeAndGo(string scene) {
        StartCoroutine(DoFade(scene));
    }

    private IEnumerator DoFade(string scene) {
        isOpaque = true;
        while (image.color.a < 1.0f) {
            yield return null;
        }
        SceneManager.LoadScene(scene);
    }
}
