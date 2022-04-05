using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour {
    [ReadOnly] public int currentHealth;
    [ReadOnly] public bool isDead = false;

    public int maxHealth;
    public Team team;
    public UnityEvent OnDie;
    public bool destroyOnDie;

    private void Awake() {
        currentHealth = maxHealth;
    }
    
    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0) {
            if(!isDead) {
                OnDie.Invoke();
                if(destroyOnDie) {
                    Destroy(gameObject);
                }
                isDead = true;
            }
        } else {
            isDead = false;
        }
    }

    public void TakeDamage(int damage) {
        currentHealth = Mathf.Max(0, currentHealth-damage);
    }
}
