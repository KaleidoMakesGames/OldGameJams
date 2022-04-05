using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(Image))]
public class Fader : MonoBehaviour
{
    public float fadeSpeed;
    public Color fadeColor;
    public float fadeAmount;
    public float currentAmount;

    private Image image;

    private void Awake() {
        image = GetComponent<Image>();
    }

    private void Update() {
        if(Application.isPlaying) {
            currentAmount = Mathf.MoveTowards(currentAmount, fadeAmount, fadeSpeed * Time.deltaTime);
        }
        image.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, currentAmount);
    }
}
