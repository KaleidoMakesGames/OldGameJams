using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace KMGPathfinding {

    public abstract class PathfindingSystem : MonoBehaviour {
        public abstract List<Vector2> GetPath(Vector2 a, Vector2 b);
    }
}