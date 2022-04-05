using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TeamAssigner))]
public class TouchDamager : MonoBehaviour {
    public float damage;

    private TeamAssigner assigner;

    private void Start() {
        assigner = GetComponent<TeamAssigner>();
    }

    private void OnCollisionStay2D(Collision2D collision) {
        TeamAssigner otherTeamAssigner = collision.collider.GetComponent<TeamAssigner>();

        if(otherTeamAssigner != null && otherTeamAssigner.team != assigner.team) {
            HealthTracker healthTracker = collision.collider.GetComponent<HealthTracker>();
            if(healthTracker != null) {
                healthTracker.TakeDamage(damage);
            }
        }
    }
}
