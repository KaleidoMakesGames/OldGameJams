using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.Events;

public class Tile : MonoBehaviour {
    public TileType type;
    public Vector2Int position;
    public UnityEvent OnScore;

    public TileGrid grid { get; set; }

    public List<Tile> GetNeighbors() {
        if(grid == null) {
            return null;
        }
        List<Tile> neighbors = new List<Tile>();
        neighbors.Add(grid.GetTileAtPosition(position + Vector2Int.right));
        neighbors.Add(grid.GetTileAtPosition(position + Vector2Int.left));
        neighbors.Add(grid.GetTileAtPosition(position + Vector2Int.up));
        neighbors.Add(grid.GetTileAtPosition(position + Vector2Int.down));
        neighbors.RemoveAll(delegate (Tile t) { return t == null; });
        return neighbors;
    }

    public bool atRest {
        get {
            return Vector2.Distance(transform.localPosition, position) <= Mathf.Epsilon;
        }

    }

    public void DoDestroy() {
        Destroy(gameObject);
    }
}

