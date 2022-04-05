using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Navigation {
    [CreateAssetMenu(menuName ="BALANCE/Navigation/Targeter Settings")]
    public class NavigationTargeterSettings : ScriptableObject {
        public float minUpdateFrequency;
        public float maxUpdateFrequency;
        public float reachedWaypointDistance;

        private void OnEnable() {
            minUpdateFrequency = Mathf.Clamp(minUpdateFrequency, 0.0f, maxUpdateFrequency);
            maxUpdateFrequency = Mathf.Clamp(maxUpdateFrequency, minUpdateFrequency, Mathf.Infinity);
        }
    }
}
