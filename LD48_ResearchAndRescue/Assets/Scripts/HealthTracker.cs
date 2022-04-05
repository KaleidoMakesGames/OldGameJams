using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthTracker : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth;
    [SerializeField] private float _currentHealth;
    public float currentHealth {
        get {
            return _currentHealth;
        }
        set {
            _currentHealth = Mathf.Clamp(value, 0, maxHealth);
        }
    }

    [Header("Shield")]
    public float maxShield;
    [SerializeField] private float _currentShield;
    public float currentShield {
        get {
            return _currentShield;
        }
        set {
            _currentShield = Mathf.Clamp(value, 0, maxShield);
        }
    }
    public float shieldRechargeTime;
    public float shieldRechargeRate;

    [Header("Death")]
    public bool destroyOnDie;
    public UnityEvent OnDie;
    public UnityEvent OnDamage;
    public UnityEvent OnShieldDamage;
    public UnityEvent OnShieldBreak;
    public UnityEvent OnShieldReform;

    public bool debug;


    private float _lastDamageTime;

    private void Awake() {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage) {
        _lastDamageTime = Time.time;

        if (currentShield > 0) {
            float remainingDamage = Mathf.Max(0.0f, damage - currentShield);
            currentShield -= damage;
            damage = remainingDamage;
            OnShieldDamage.Invoke();
            if(currentShield == 0) {
                OnShieldBreak.Invoke();
            }
        }

        if(damage == 0) {
            return;
        }

        OnDamage.Invoke();
        currentHealth -= damage;

        if(currentHealth == 0) {
            OnDie.Invoke();
            if(destroyOnDie) {
                Destroy(gameObject);
            }
        }
    }

    private void Update() {
        if(Time.time - _lastDamageTime >= shieldRechargeTime && maxShield > 0) {
            if(currentShield == 0) {
                OnShieldReform.Invoke();
            }
            currentShield += shieldRechargeRate * Time.deltaTime;
        }

        if (debug) {
            if (Input.GetKeyDown(KeyCode.K)) {
                TakeDamage(1.0f);
            }
            if (Input.GetKeyDown(KeyCode.H)) {
                Heal(1.0f);
            }
        }
    }

    public void Heal(float amount) {
        currentHealth += amount;
    }
}
