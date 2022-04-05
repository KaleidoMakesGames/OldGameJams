using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class NavigationAgent : MonoBehaviour
{
    [HideInInspector] public Vector2Int destination;

    private MovementController movementController;
    private Vector2Int lastDestination;

    private List<Vector2Int> pathToFollow;

    private float lastComputeTime;

    private void Awake() {
        movementController = GetComponent<MovementController>();
        lastDestination = destination;
    }

    private void Start() {
        destination = movementController.gridElement.position;
        RecomputePath();
    }

    public bool IsAtDestination() {
        if(!movementController.isMovingToTile && movementController.gridElement.position == destination) {
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (lastDestination != destination || (Time.time - lastComputeTime) >= 2.0f) {
            RecomputePath();
            lastDestination = destination;
        }

        FollowPath();
    }

    private void FollowPath() {
        if(pathToFollow == null || pathToFollow.Count == 0) {
            movementController.drive = Vector2Int.zero;
            return;
        }
        if (!movementController.isMovingToTile) {
            Vector2Int currentPosition = movementController.gridElement.position;
            Vector2Int drive = pathToFollow[0] - currentPosition;
            movementController.drive = drive;
            if (currentPosition == pathToFollow[0]) {
                pathToFollow.RemoveAt(0);
            }
        } else {
            movementController.drive = Vector2Int.zero;
        }
    }

    public void RecomputePath() {
        Navigator.ComputePath(movementController, destination, delegate (List<Vector2Int> path) {
            pathToFollow = path;

            if (pathToFollow == null) {
                Debug.Log("No path found.");
            }
        });
        lastComputeTime = Time.time;
    }

    private void OnDrawGizmos() {
        if(pathToFollow != null && pathToFollow.Count > 0 && WorldGrid.Instance != null) {
            for(int i = 1; i < pathToFollow.Count; i++) {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(WorldGrid.Instance.grid.GetCellCenterWorld((Vector3Int)pathToFollow[i - 1]), WorldGrid.Instance.grid.GetCellCenterWorld((Vector3Int)pathToFollow[i]));
            }
            Gizmos.DrawWireSphere(WorldGrid.Instance.grid.GetCellCenterWorld((Vector3Int)pathToFollow[pathToFollow.Count - 1]), 0.5f);
        }
    }
}
