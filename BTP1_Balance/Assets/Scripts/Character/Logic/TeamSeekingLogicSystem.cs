using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Navigation;

namespace Logic {
    [CreateAssetMenu(menuName ="BALANCE/Logic/Team Seeking System")]
    public class TeamSeekingLogicSystem : LogicSystem {
        public Team teamToSeek;
        public float seekRange;

        public override void UpdateLogic(CharacterLogicController controller) {
            NavigationTargeter targeter = controller.GetComponent<NavigationTargeter>();
            if(targeter == null) {
                return;
            }

            if(targeter.target != null) {
                return;
            }

            Transform target = null;
            float closestDistance = Mathf.Infinity;
            
            foreach(Collider2D collider in Physics2D.OverlapCircleAll(controller.transform.position, seekRange)) {
                CharacterTeamAssigner assigner = collider.GetComponent<CharacterTeamAssigner>();
                if(assigner != null) {
                    if (assigner.team == teamToSeek) {
                        float distance = Vector2.Distance(controller.transform.position, assigner.transform.position);
                        if (distance < closestDistance) {
                            target = assigner.transform;
                            closestDistance = distance;
                        }
                    }
                }
            }
            targeter.target = target;
            targeter.RecomputeWaypoints();
        }
    }
}
