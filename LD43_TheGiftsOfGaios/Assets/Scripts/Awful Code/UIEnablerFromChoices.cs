using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEnablerFromChoices : MonoBehaviour {
    public bool flyIcon;
    public bool dashIcon;
    public bool timeIcon;
    public bool shootIcon;
    public bool expandedVisionIcon;

    // Use this for initialization
    void Awake() {
        bool shouldEnable = false;
        if(flyIcon && PlayerChoices.Instance.canInfiniJump) {
            shouldEnable = true;
        }
        if (dashIcon && PlayerChoices.Instance.canDash) {
            shouldEnable = true;
        }
        if (timeIcon && PlayerChoices.Instance.canSlowTime) {
            shouldEnable = true;
        }
        if (shootIcon && PlayerChoices.Instance.canAttack) {
            shouldEnable = true;
        }
        if (expandedVisionIcon && PlayerChoices.Instance.hasFullVision) {
            shouldEnable = true;
        }

        gameObject.SetActive(shouldEnable);
    }
}
