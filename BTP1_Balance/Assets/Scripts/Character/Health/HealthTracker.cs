using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthTracker : MonoBehaviour {
    public float maxHealth;

    public float currentHealth;

    public UnityEvent OnDied;

    private void Update() {
        if (currentHealth <= 0) {
            OnDied.Invoke();
        }
    }

    public void TakeDamage(float damage) {
        currentHealth = Mathf.Clamp(currentHealth - Mathf.Abs(damage), 0.0f, maxHealth);
    }

    public void RestoreHealth(float health) {
        currentHealth = Mathf.Clamp(currentHealth + Mathf.Abs(health), 0.0f, maxHealth);
    }

    public void FullRestore() {
        currentHealth = maxHealth;
    }

    private void OnValidate() {
        currentHealth = Mathf.Clamp(currentHealth, 0.0f, maxHealth);
    }
}