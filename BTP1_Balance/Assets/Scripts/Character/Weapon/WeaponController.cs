using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponController : MonoBehaviour {
    public Weapon currentWeapon;
    public WeaponBehavior behavior;

    public UnityEvent OnUseWeapon;

    public bool isOnCooldown { get; private set; }

    private void Update() {
        if(behavior != null) {
            behavior.DoBehavior(this);
        }
    }

    public void UseWeapon() {
        if(currentWeapon == null || isOnCooldown) {
            return;
        }

        OnUseWeapon.Invoke();

        CharacterTeamAssigner myTeam = GetComponent<CharacterTeamAssigner>();
        
        foreach(Collider2D collider in GetCollidersInWeaponRange()) {
            HealthTracker tracker = collider.GetComponentInParent<HealthTracker>();
            if (tracker != null) {
                CharacterTeamAssigner otherTeam = collider.GetComponentInParent<CharacterTeamAssigner>();
                if (otherTeam == null || myTeam == null || myTeam.team != otherTeam.team) {
                    tracker.TakeDamage(currentWeapon.damage);
                    
                    Rigidbody2D rb = collider.GetComponentInParent<Rigidbody2D>();
                    if (rb != null) {
                        rb.AddForce((rb.position - (Vector2)transform.position).normalized * currentWeapon.force, ForceMode2D.Impulse);
                    }
                }
            }
        }

        StartCoroutine(WaitForCooldown(currentWeapon.cooldown));
    }

    public bool AreEnemiesInRange() {
        CharacterTeamAssigner myTeam = GetComponent<CharacterTeamAssigner>();

        foreach (Collider2D collider in GetCollidersInWeaponRange()) {
            HealthTracker tracker = collider.GetComponentInParent<HealthTracker>();
            if (tracker != null) {
                CharacterTeamAssigner otherTeam = collider.GetComponentInParent<CharacterTeamAssigner>();
                if (otherTeam == null || myTeam == null || myTeam.team != otherTeam.team) {
                    return true;
                }
            }
        }
        return false;
    }

    private List<Collider2D> GetCollidersInWeaponRange() {
        if (currentWeapon != null) {
            return new List<Collider2D>(Physics2D.OverlapCircleAll(transform.position, currentWeapon.range));
        }
        return null;
    }

    private IEnumerator WaitForCooldown(float cooldown) {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldown);
        isOnCooldown = false;
    }

    private void OnDrawGizmos() {
        if(currentWeapon != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, currentWeapon.range);
        }
    }
}
