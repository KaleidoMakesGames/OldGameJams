using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthTracker : MonoBehaviour
{
    public int maxHealth;
    public float invulnerabilityOnDamageTakenTime;
    [ReadOnly] public int currentHealth;
    [ReadOnly] public bool isInvulnerable;

    public UnityEvent OnDamaged;
    public UnityEvent OnHealed;
    public UnityEvent OnDeath;

    private void Start() {
        currentHealth = maxHealth;
    }

    private void Update() {
        var animator = GetComponent<Animator>();
        if(animator) {
            animator.SetBool("Invulnerable", isInvulnerable);
        }
    }

    public void TakeDamage(int amount) {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        StartCoroutine(DoInvulnerable());
        OnDamaged.Invoke();
        if(currentHealth <= 0) {
            OnDeath.Invoke();
        }
    }

    private IEnumerator DoInvulnerable() {
        if(isInvulnerable) {
            yield break;
        }
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityOnDamageTakenTime);
        isInvulnerable = false;
    }

    public void Heal(int amount) {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        OnHealed.Invoke();
    }
}
