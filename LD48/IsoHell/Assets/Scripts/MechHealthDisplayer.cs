using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechHealthDisplayer : MonoBehaviour {
    public HealthTracker healthTracker;
    public Image healthDial;
    public Image shieldDial;
    public Image damageDial;
    public Image healDial;

    public float healthAngle;
    public bool clockWise;

    [Header("Damage Lerp")]
    public float damageLerpSpeed;

    private float _lerpedHealth;

    private void Start() {
        _lerpedHealth = healthTracker.currentHealth;
    }

    // Update is called once per frame
    void Update() {
        float healthPercentage = healthTracker == null ? 0.0f : (healthTracker.currentHealth / healthTracker.maxHealth);
        float shieldPercentage = healthTracker.currentShield / healthTracker.maxHealth;
        float lerpedPercentage = _lerpedHealth / healthTracker.maxHealth;

        healthDial.fillClockwise = clockWise;
        damageDial.fillClockwise = clockWise;
        healDial.fillClockwise = !clockWise;
        shieldDial.fillClockwise = !clockWise;

        healthDial.fillAmount = healthPercentage;
        shieldDial.fillAmount = shieldPercentage;
        damageDial.fillAmount = lerpedPercentage - healthPercentage;
        healDial.fillAmount = healthPercentage - lerpedPercentage;

        healthDial.transform.localEulerAngles = new Vector3(healthDial.transform.localEulerAngles.x,
            healthDial.transform.localEulerAngles.y,
            healthAngle);

        shieldDial.transform.localEulerAngles = new Vector3(shieldDial.transform.localEulerAngles.x,
            shieldDial.transform.localEulerAngles.y,
            (clockWise ? 1.0f : -1.0f) * (1-healthDial.fillAmount) * 360.0f + healthAngle);

        damageDial.transform.localEulerAngles = new Vector3(damageDial.transform.localEulerAngles.x,
            damageDial.transform.localEulerAngles.y,
            (clockWise ? 1.0f : -1.0f) * (1 - healthDial.fillAmount) * 360.0f + healthAngle);

        healDial.transform.localEulerAngles = new Vector3(healDial.transform.localEulerAngles.x,
            healDial.transform.localEulerAngles.y,
            (clockWise ? 1.0f : -1.0f) * (1 - healthDial.fillAmount) * 360.0f + healthAngle);

        _lerpedHealth = Mathf.MoveTowards(_lerpedHealth, healthTracker.currentHealth, damageLerpSpeed * Time.deltaTime * healthTracker.maxHealth);
    }
}
