using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoDisplayer : MonoBehaviour
{
    public TextMeshProUGUI textField;
    public Image bulletIndicator;

    public int lowAmmoThreshold;

    public AnimationCurve outOfAmmoAnimation;

    public WeaponController weaponController;
    
    // Update is called once per frame
    void Update()
    {
        var c = bulletIndicator.color;
        c.a = weaponController.bulletsInClip < lowAmmoThreshold ? outOfAmmoAnimation.Evaluate(Time.time) : 1.0f;
        bulletIndicator.color = c;

        textField.text = weaponController.bulletsInClip.ToString();
    }
}
