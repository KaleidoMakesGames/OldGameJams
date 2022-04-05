using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteInEditMode]
public class FlaskContents : MonoBehaviour
{
    public SpriteRenderer contentsSprite;
    public Potion contents;
    public TextMeshProUGUI textField;
    public GameObject hover;

    private void Update() {
        if (contentsSprite != null) {
            if (contents != null) {
                contentsSprite.enabled = true;
                contentsSprite.color = contents.potionColor;
            } else {
                contentsSprite.enabled = false;
            }
        }

        if (textField != null) {
            if (contents != null) {
                hover.gameObject.SetActive(true);
                textField.text = contents.FString();
            } else {
                hover.gameObject.SetActive(false);
                textField.text = "EMPTY";
            }
        }
    }
}
