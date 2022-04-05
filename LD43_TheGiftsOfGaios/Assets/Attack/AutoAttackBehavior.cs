using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GoG/Attack Behavior/Auto Attack Behavior")]
public class AutoAttackBehavior : AttackBehavior {
    public float attackRadius;

    public override void DoBehavior(CharacterAttackController controller) {
        List<Collider2D> possibleTargets = new List<Collider2D>();

        Vector2 selfPosition = controller.transform.position;

        Team selfTeam = controller.GetComponent<TeamAssigner>().team;

        foreach (Collider2D collider in Physics2D.OverlapCircleAll(selfPosition, attackRadius)) {
            TeamAssigner assigner = collider.GetComponent<TeamAssigner>();
            if (assigner != null && selfTeam != assigner.team) {
                possibleTargets.Add(collider);
            }
        }

        float closestDistance = Mathf.Infinity;
        Collider2D closestTarget = null;

        foreach (Collider2D possibleTarget in possibleTargets) {
            // Check if we have line of sight
            Vector2 targetPosition = possibleTarget.transform.position;
            Vector2 direction = targetPosition - selfPosition;
            float distance = direction.magnitude;

            bool lineOfSight = true;
            foreach(RaycastHit2D hit in Physics2D.RaycastAll(controller.transform.position, direction, distance)) {
                if(hit.collider.gameObject.GetComponent<VisionOccluder>() != null) {
                    lineOfSight = false;
                }
            }

            if(lineOfSight && distance <= closestDistance) {
                closestTarget = possibleTarget;
                closestDistance = distance;
            }
        }

        if(closestTarget != null) {
            controller.Attack(closestTarget.transform.position);
        }
    }

    public override void DrawAttackGizmos(CharacterAttackController controller) {
        base.DrawAttackGizmos(controller);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controller.transform.position, attackRadius);
    }
}
