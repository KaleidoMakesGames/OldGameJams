using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Navigation {
    [CreateAssetMenu(menuName ="BALANCE/Navigation/Area Settings")]
    public class NavigationAreaSettings : ScriptableObject {
        [Header("Navigatable Area Settings")]
        [Tooltip("Spacing between nodes in the navigatable area.")]
        public float nodeSpacing;

        public Vector2 offset;
    }
}