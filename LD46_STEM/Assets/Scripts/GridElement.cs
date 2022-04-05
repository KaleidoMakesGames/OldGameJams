using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridElement : MonoBehaviour
{
    private Vector2Int _position;
    public Vector2Int position {
        get {
            return _position;
        }
        set {
            WorldGrid.Instance.RemoveFromMap(this);
            _position = value;
            WorldGrid.Instance.AddToMap(this);
        }
    }
    public bool isObstacle;
    public bool isSupport = false;

    private void Awake() {
        position = (Vector2Int)WorldGrid.Instance.grid.WorldToCell(transform.position);
    }

    private void Start() {
        transform.position = WorldGrid.Instance.grid.GetCellCenterWorld((Vector3Int)position);    
    }

    private void OnDestroy() {
        // Does this happen right when Destroy is called?
        WorldGrid.Instance.RemoveFromMap(this);
    }
}
