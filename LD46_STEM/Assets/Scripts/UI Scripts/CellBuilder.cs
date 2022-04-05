using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class CellBuilder : MonoBehaviour
{
    [HideInInspector] public CellInfo selectedCellPrefab;
    public List<CellInfo> availableCells;

    public ProteinTracker tracker;

    private MovementController movementController;

    private void Awake() {
        movementController = GetComponent<MovementController>();
    }

    private void Start() {
        selectedCellPrefab = availableCells[0];
        lastPlace = movementController.gridElement.position;
    }

    public bool HasProtein(CellInfo cell) {
        if(tracker.currentProtein >= cell.proteinCost) {
            return true;
        }
        return false;
    }


    private Vector2Int lastDelta;
    private Vector2Int lastPlace;

    private void Update() {
        if(movementController.gridElement.position != lastPlace) {
            lastDelta = movementController.gridElement.position - lastPlace;
        }
        lastPlace = movementController.gridElement.position;
    }

    private Vector2Int? ComputeValidDrive() {
        // Have we moved at all?
        if (lastDelta != Vector2Int.zero) {
            // Can we keep moving forward?
            if (movementController.CanDoMove(movementController.gridElement.position, lastDelta)) {
                return lastDelta;
            }

            // Nope. What about backward?
            if (movementController.CanDoMove(movementController.gridElement.position, -lastDelta)) {
                return -lastDelta;
            }
        }

        // Bummer. Let's find first unoccupied spot.
        for(int x = -1; x <= 1; x++) {
            for(int y = -1; y <= 1; y++) {
                if(x == 0 && y == 0) {
                    continue;
                }
                // Can we move to that spot?
                if (movementController.CanDoMove(movementController.gridElement.position, new Vector2Int(x, y))) {
                    return new Vector2Int(x, y);
                }
            }
        }

        // Can't move anywhere... Can't build right now.
        return null;
    }

    public bool HasRoom() {
        return ComputeValidDrive().HasValue;
    }

    private bool isBuilding = false;
    public void BuildCell(CellInfo cell) {
        if (!isBuilding) {
            StartCoroutine(BuildWhenStill(cell));
        }
    }

    private IEnumerator BuildWhenStill(CellInfo cell) {
        isBuilding = true;
        while(movementController.isMovingToTile) {
            yield return null;
        }
        if(HasProtein(cell) && HasRoom()) {
            tracker.currentProtein -= cell.proteinCost;

            CellInfo newCell = Instantiate(cell.gameObject).GetComponent<CellInfo>();
            newCell.gridElement.position = movementController.gridElement.position;

            Vector2Int drive = ComputeValidDrive().Value;
            movementController.drive = drive;
            movementController.MoveNow();
        }
        while(movementController.isMovingToTile) {
            yield return null;
        }
        isBuilding = false;

        foreach (NavigationAgent a in FindObjectsOfType<NavigationAgent>()) {
            a.RecomputePath();
        }
    }
}
