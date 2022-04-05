using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthTracker : MonoBehaviour {
    public float maxHealth = 100.0f;

    public float currentHealth { get; private set; }

    public float invulnerabilityTime;

    public UnityEvent OnDamageTaken;
    public UnityEvent OnDie;

    private float lastDamageTakenTime = Mathf.NegativeInfinity;

    private void Start() {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage) {
        if (Time.time - lastDamageTakenTime >= invulnerabilityTime || damage == Mathf.Infinity) {
            currentHealth -= damage;
            if (currentHealth <= 0) {
                OnDie.Invoke();
            }
            OnDamageTaken.Invoke();
            lastDamageTakenTime = Time.time;
        }
    }

    public void DestroyHealthObject() {
        Destroy(gameObject);
    }
}
