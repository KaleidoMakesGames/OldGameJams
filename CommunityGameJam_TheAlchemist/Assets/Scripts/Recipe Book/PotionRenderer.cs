using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode]
public class PotionRenderer : MonoBehaviour
{
    public Potion potion;
    public Image image;
    public TextMeshProUGUI textField;

    private void Update() {
        if(image != null && potion != null) {
            image.color = potion.potionColor;
            textField.text = potion.name.ToUpper();
        }
    }
}
