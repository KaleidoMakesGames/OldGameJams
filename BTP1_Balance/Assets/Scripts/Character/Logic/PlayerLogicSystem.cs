using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logic;

[CreateAssetMenu(menuName ="BALANCE/Logic/Player Logic")]
public class PlayerLogicSystem : LogicSystem {
    public float visionRange;
    public float enemyAttackRange;
    
    public float panicPercentage;

    public float excitementGainedWhenInCombatPerSecond;
    public float panicExcitementMultiplier;

    public override void UpdateLogic(CharacterLogicController controller) {
        NavigationTargeter targeter = controller.GetComponent<NavigationTargeter>();
        WaypointChainFollower follower = controller.GetComponent<WaypointChainFollower>();
        HealthTracker tracker = controller.GetComponent<HealthTracker>();
        ExcitementTracker excitementTracker = controller.GetComponent<ExcitementTracker>();
        CharacterTeamAssigner teamAssigner = controller.GetComponent<CharacterTeamAssigner>();
        WeaponController weaponController = controller.GetComponent<WeaponController>();

        bool canAttack = !weaponController.isOnCooldown;
        bool isPanicked = tracker.currentHealth / tracker.maxHealth <= panicPercentage;
        
        List<CharacterTeamAssigner> enemiesInVision = new List<CharacterTeamAssigner>();
        List<Potion> potionsInVision = new List<Potion>();

        foreach(Collider2D collider in Physics2D.OverlapCircleAll(controller.transform.position, visionRange)) {
            CharacterTeamAssigner otherTeamAssigner = collider.GetComponent<CharacterTeamAssigner>();
            Potion potion = collider.GetComponent<Potion>();
            
            if (otherTeamAssigner != null && otherTeamAssigner.team != teamAssigner.team) {
                enemiesInVision.Add(otherTeamAssigner);
            }

            if(potion != null) {
                potionsInVision.Add(potion);
            }
        }

        List<CharacterTeamAssigner> enemiesInAttackRange = ClosestUtility.ThresholdList(controller.transform, enemiesInVision, enemyAttackRange);

        bool inCombat = enemiesInAttackRange.Count > 0;

        if (inCombat) {
            excitementTracker.GainExcitement(excitementGainedWhenInCombatPerSecond * Time.deltaTime * (isPanicked ? panicExcitementMultiplier : 1.0f));
        }

        if (inCombat && !isPanicked && canAttack) {
            targeter.target = ClosestUtility.GetClosestInList(controller.transform, enemiesInAttackRange).transform;
            return;
        }

        bool canGetToPotion = potionsInVision.Count > 0;
        bool wantsPotion = tracker.currentHealth < tracker.maxHealth;

        if(wantsPotion && canGetToPotion) {
            targeter.target = ClosestUtility.GetClosestInList(controller.transform, potionsInVision).transform;
            return;
        }

        targeter.target = follower.currentWaypoint;
    }

    public override void DrawLogicGizmos(CharacterLogicController controller) {
        base.DrawLogicGizmos(controller);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(controller.transform.position, visionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controller.transform.position, enemyAttackRange);
    }
}
