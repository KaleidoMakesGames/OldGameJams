using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CitadelsPrototype {
    [System.Serializable]
    public class Team {
        public string name;
    }

    [System.Serializable]
    public class UnitSpecs {
        public float movementSpeed;
        public int attack;
        public int defense;
        public int radius;
        public int health;
        public float attackSpeed;
        public bool canChangeBuildPosition;
        public List<UnitBehavior> availableBehaviors;
    }

    [System.Serializable]
    public class BoardUnit {
        // Config
        public UnitSpecs unitSpecs;
        public Team team;
        public Vector2 buildPosition;

        // State
        public int currentHealth;
        public Vector2 currentPosition;
        public Vector2 attackGoalPosition;
        public BoardUnit attackGoalTarget;
        public BoardUnit currentTarget;
        public UnitBehavior behavior;
    }

    [System.Serializable]
    public class Board {
        public List<BoardUnit> unitsOnBoard;

        public List<BoardUnit> FindUnits(Vector2 position, float radius) {
            return unitsOnBoard.FindAll(delegate (BoardUnit unit) {
                return Vector2.Distance(unit.currentPosition, position) <= (radius + unit.unitSpecs.radius);
            });
        }
    }

    public abstract class UnitBehavior {
        public bool isActive;
        public abstract void DoBehavior(BoardUnit unit, Board board);
    } 

    public abstract class Superbehavior : UnitBehavior {
        public List<UnitBehavior> subBehaviors;
        public override void DoBehavior(BoardUnit unit, Board board) {
            subBehaviors.FindAll(x => x.isActive).ForEach(x => DoBehavior(unit, board));
        }
    }

    public class SoldierBehavior : Superbehavior {
        public override void DoBehavior(BoardUnit unit, Board board) {
            base.DoBehavior(unit, board);
        }
    }


    public class PathingBehavior {

    }
}