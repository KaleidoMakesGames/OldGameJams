using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NavigationTargeter))]
public class HealthBarTargetEnemyLighter : MonoBehaviour {
    public RectTransform healthBar;

    private NavigationTargeter targeter;

    private void Awake() {
        targeter = GetComponent<NavigationTargeter>();
        healthBar.gameObject.SetActive(targeter.target != null);
    }

    private void Update() {
        healthBar.gameObject.SetActive(targeter.target != null);
    }
}
