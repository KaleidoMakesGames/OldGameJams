using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridElement))]
public class MovementController : MonoBehaviour
{
    private Vector2Int _drive;

    public Vector2Int drive {
        get {
            return _drive;
        }
        set {
            _drive = value;
            _drive.Clamp(-Vector2Int.one, Vector2Int.one);
        }
    }

    [Tooltip("Tiles per second.")]
    public float movementSpeed = 1.0f;

    public bool isMovingToTile { get; private set; }

    public GridElement gridElement { get; private set; }

    private void Awake() {
        gridElement = GetComponent<GridElement>();
        isMovingToTile = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMovingToTile) {
            transform.position = WorldGrid.Instance.grid.GetCellCenterWorld((Vector3Int)gridElement.position);
            MoveNow();
        }
    }

    public void MoveNow() {
        if (drive.magnitude > 0 && !isMovingToTile) {
            StartCoroutine(DoMove());
        }
    }

    IEnumerator DoMove() {
        Vector2Int startTile = gridElement.position;
        if(!CanDoMove(startTile, drive)) {
            yield break;
        }
        isMovingToTile = true;

        Vector2Int goalTile = startTile + drive;
        Vector3 goalPosition = WorldGrid.Instance.grid.GetCellCenterWorld((Vector3Int)goalTile);
        gridElement.position = goalTile;

        while (true) {
            float distance = Vector3.Distance(transform.position, goalPosition);
            if(Mathf.Approximately(distance, 0.0f)) {
                isMovingToTile = false;
                yield break;
            } else {
                transform.position = Vector3.MoveTowards(transform.position, goalPosition, movementSpeed * Time.deltaTime);
            }
            yield return null;
        }
    }

    public bool CanDoMove(Vector2Int from, Vector2Int drive) {
        Vector2Int goalTile = from + drive;
        if (!WorldGrid.Instance.CanOccupy(goalTile)) {
            return false;
        }
        if (drive.magnitude > 1.0f) {
            // Moving diagonal. Check adjacent cells
            Vector2Int adjacentA = from + new Vector2Int(drive.x, 0);
            Vector2Int adjacentB = from + new Vector2Int(0, drive.y);
            if (!WorldGrid.Instance.CanOccupy(adjacentA) || !WorldGrid.Instance.CanOccupy(adjacentB)) {
                return false;
            }
        }
        return true;
    }
}
