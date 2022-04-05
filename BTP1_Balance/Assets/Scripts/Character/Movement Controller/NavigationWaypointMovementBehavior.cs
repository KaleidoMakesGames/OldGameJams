using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Navigation;

namespace Character {
    [CreateAssetMenu(menuName = "BALANCE/Navigation Waypoint Movement Behavior")]
    public class NavigationWaypointMovementBehavior : MovementBehavior {
        public override void DoBehavior(CharacterMovementController controller) {
            NavigationTargeter targeter = controller.GetComponent<NavigationTargeter>();

            if(targeter == null || targeter.waypoints == null) {
                return;
            }

            if (targeter.waypoints.Count > 0) {
                Vector2 destination = targeter.waypoints.Peek();
                float deltaDistance = ((Vector2)targeter.self.position - destination).magnitude;
                Vector2 nextPosition = Vector2.MoveTowards(targeter.self.position, destination, Mathf.Min((controller.movementSpeed * Time.fixedDeltaTime), deltaDistance));
                Vector2 delta = nextPosition - (Vector2)targeter.self.position;

                controller.MoveByDelta(delta);
            }
        }
    }
}
