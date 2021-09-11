using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KMGMovement2D {
    public interface IScriptableObstacle {
        bool IsObstacle(CharacterMover2D mover);
    }
}