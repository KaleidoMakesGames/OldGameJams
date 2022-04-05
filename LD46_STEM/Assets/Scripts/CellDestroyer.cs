using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellDestroyer : MonoBehaviour
{
    public ProteinTracker tracker;

    public Protein proteinPrefab;

    private MovementController movementController;

    private void Awake() {
        movementController = GetComponent<MovementController>();
    }

    public bool InRangeToDestroy(CellInfo cell) {
        if(Vector2Int.Distance(cell.gridElement.position, movementController.gridElement.position) <= Mathf.Sqrt(2)) {
            return true;
        }
        return false;
    }

    public bool IsLastSupportingCell(CellInfo cell) {
        FibroblastCTSpawner spawner = cell.GetComponent<FibroblastCTSpawner>();

        if(spawner == null) {
            return false;
        }

        // Its a fibroblast, lets see if we can destroy it.
        // Decide if we can destroy this fibroblast

        // Fibroblasts can be destroyed if there is nothing on their connective tissue (besides themselves) in a single-supported square

        foreach (GridElement ct in spawner.myCTs) {
            // Is this supported by more than one CT?
            bool hasDoubleSupport = false;
            foreach (GridElement e in WorldGrid.Instance.ElementsAtPosition(ct.position)) {
                if (e == ct) {
                    // This is just my support.
                    continue;
                }
                if (e.GetComponent<ConnectiveTissue>() != null) {
                    // This is double-supported. It's fine.
                    hasDoubleSupport = true;
                    break;
                }
            }
            // If its double supported, let's just look at the next one.
            if (hasDoubleSupport) {
                continue;
            } else {
                // It's not double supported. Is there anything on here that needs to be preserved?
                foreach (GridElement e in WorldGrid.Instance.ElementsAtPosition(ct.position)) {
                    // Is it a cell (or us?)
                    if (e.GetComponent<CellInfo>() != null || e.GetComponent<CellDestroyer>() != null) {
                        // There's a cell on this single-supported square!
                        if (e == spawner.myGridElement) {
                            // Don't mind destroying the fibroblast itself.
                            continue;
                        } else {
                            // Don't destroy
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public void DestroyCell(CellInfo cell) {
        foreach(NavigationAgent a in FindObjectsOfType<NavigationAgent>()) {
            a.RecomputePath();
        }

        if(InRangeToDestroy(cell) && !IsLastSupportingCell(cell)) {
            for (int i = 0; i < cell.proteinCost; i++) {
                GridElement e = Instantiate(proteinPrefab.gameObject).GetComponent<GridElement>();
                e.position = cell.gridElement.position;
            }
            Destroy(cell.gameObject);
        }
    }
}
