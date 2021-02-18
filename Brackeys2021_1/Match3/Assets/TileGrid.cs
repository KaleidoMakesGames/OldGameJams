using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Dynamic;
using System.Collections;
using UnityEngine.Events;
using System.Text.RegularExpressions;

public class TileGrid : MonoBehaviour {
    public UnityEvent OnScore;

    public bool onlyAllowScoringMoves;

    [SerializeField] private Vector2Int size;

    public float width {
        get {
            return size.x;
        }
    }
    public float height {
        get {
            return size.y;
        }
    }

    private List<Tile> _tiles;

    private (Vector2Int, Vector2Int)? lastSwap;

    private void Awake() {
        _tiles = new List<Tile>();
    }

    private void Start() {
        transform.position = -((Vector2)size) / 2.0f;
        StartCoroutine(WaitForMoveToFinish());
    }

    public Vector2Int WorldToTile(Vector2 world) {
        return Vector2Int.FloorToInt(transform.InverseTransformPoint(world));
    }

    public Vector2 TileToWorld(Vector2Int tile) {
        return transform.TransformPoint((Vector2)tile + Vector2.one * 0.5f);
    }

    public void AddTile(Tile t) {
        t.grid = this;
        _tiles.Add(t);
    }

    public void RemoveTile(Tile t) {
        _tiles.Remove(t);
    }

    public Tile GetTileAtPosition(Vector2Int position) {
        foreach (Tile t in _tiles) {
            if (t.position == position) {
                return t;
            }
        }
        return null;
    }

    public List<Tile> FindMatchedTiles() {
        List<Tile> matches = new List<Tile>();
        foreach (Tile tile in _tiles) {
            var left = FindContiguous(tile, new Vector2Int(-1, 0));
            var right = FindContiguous(tile, new Vector2Int(1, 0));
            var up = FindContiguous(tile, new Vector2Int(0, 1));
            var down = FindContiguous(tile, new Vector2Int(0, -1));
            if (left.Count >= 3) {
                matches = new List<Tile>(matches.Union(left));
            }
            if (right.Count >= 3) {
                matches = new List<Tile>(matches.Union(right));
            }
            if (up.Count >= 3) {
                matches = new List<Tile>(matches.Union(up));
            }
            if (down.Count >= 3) {
                matches = new List<Tile>(matches.Union(down));
            }
        }
        return matches;
    }

    public bool IsInBounds(Vector2Int pos) {
        return pos.x >= 0 && pos.y >= 0 && pos.x < width && pos.y < height;
    }

    public bool AtRest() {
        return _tiles.All(delegate (Tile t) { return t.atRest; });
    }

    public List<Tile> FindContiguous(Tile startTile, Vector2Int direction) {
        List<Tile> contiguous = new List<Tile>();

        Vector2Int currentPosition = startTile.position;
        while (IsInBounds(currentPosition)) {
            Tile neighbor = GetTileAtPosition(currentPosition);
            if (neighbor != null && neighbor.type == startTile.type) {
                contiguous.Add(neighbor);
            } else {
                break;
            }
            currentPosition += direction;
        }
        return contiguous;
    }

    public bool CanSwap(Vector2Int a, Vector2Int b) {
        if(!AtRest()) {
            return false;
        }
        if(GetTileAtPosition(a) == null || GetTileAtPosition(b) == null) {
            return false;
        }
        return Vector2Int.Distance(a, b) == 1;
    }

    public void SwapTiles(Vector2Int a, Vector2Int b) {
        var tileA = GetTileAtPosition(a);
        var tileB = GetTileAtPosition(b);
        tileA.position = b;
        tileB.position = a;
        lastSwap = (a, b);
        StartCoroutine(WaitForMoveToFinish());
    }

    private IEnumerator WaitForMoveToFinish() {
        while(!AtRest()) {
            yield return null;
        }
        ProcessMove();
    }

    private void ProcessMove() {
        var matches = FindMatchedTiles();
        if(matches.Count > 0) {
            lastSwap = null;
            foreach(var match in matches) {
                match.OnScore.Invoke();
                RemoveTile(match);
            }
            SlideDown();
            OnScore.Invoke();
            StartCoroutine(WaitForMoveToFinish());
        } else {
            if (onlyAllowScoringMoves && lastSwap.HasValue) {
                UndoSwap();
            }
        }
    }

    public void UndoSwap() {
        var tileA = GetTileAtPosition(lastSwap.Value.Item1);
        var tileB = GetTileAtPosition(lastSwap.Value.Item2);
        tileA.position = lastSwap.Value.Item2;
        tileB.position = lastSwap.Value.Item1;
    }

    public List<Vector2Int> GetEmptySpots() {
        List<Vector2Int> emptySpots = new List<Vector2Int>();
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (GetTileAtPosition(new Vector2Int(x, y)) == null) {
                    emptySpots.Add(new Vector2Int(x, y));
                }
            }
        }
        return emptySpots;
    }

    public void SlideDown() {
        for (int x = 0; x < width; x++) { 
            for(int y = 0; y < height; y++) { 
                Tile t = GetTileAtPosition(new Vector2Int(x, y));
                if (t != null) {
                    SlideDown(t);
                }
            }
        }
    }

    private void SlideDown(Tile t) {
        while (IsInBounds(t.position + Vector2Int.down) && GetTileAtPosition(t.position + Vector2Int.down) == null) {
            t.position += Vector2Int.down;
        }
    }
}
