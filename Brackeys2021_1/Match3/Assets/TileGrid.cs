using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour {
    public Vector2Int size;
    public List<Tile> tilePrefabs;

    public float fallSpeed;

    private List<Tile> _tiles;

    private Tile tileA;

    private bool isAnimating;

    private void Awake() {
        _tiles = new List<Tile>();
        isAnimating = false;
    }

    private void Start() {
        Refill();
        transform.position = new Vector2(-size.x / 2.0f, -size.y / 2.0f);
        StartCoroutine(DoAnimation());
    }

    private void Update() {
        if (isAnimating) {
            return;
        }
        if (Input.GetMouseButtonUp(0)) {
            Tile t = GetTileUnderMouse();
            if (t == null) {
                tileA = null;
            } else {
                if (tileA == null) {
                    tileA = t;
                } else if (tileA != t) {
                    Vector2Int tempPos = tileA.position;
                    tileA.position = t.position;
                    t.position = tempPos;
                    tileA = null;
                    DoAnimation();
                }
            }
        }
    }

    private IEnumerator DoAnimation() {
        if (isAnimating) {
            yield return null;
        }
        isAnimating = true;

        DoPostAnimation();
        isAnimating = false;
    }

    private void DoPostAnimation() {
        bool destroyed = false;
        foreach (Tile toDestroy in CheckPuzzle()) {
            DestroyTile(toDestroy);
        }
    }

    private void DestroyTile(Tile t) {
        _tiles.Remove(t);
        Destroy(t.gameObject);
    }

    private List<Tile> CheckPuzzle() {
        List<Tile> tilesToDestroy = new List<Tile>();
        foreach (Tile baseTile in _tiles) {
            var left = FindContiguous(baseTile, new Vector2Int(-1, 0));
            var right = FindContiguous(baseTile, new Vector2Int(1, 0));
            var up = FindContiguous(baseTile, new Vector2Int(0, 1));
            var down = FindContiguous(baseTile, new Vector2Int(0, -1));
            if (left.Count >= 3 || right.Count >= 3 || up.Count >= 3 || down.Count >= 3) {
                tilesToDestroy = new List<Tile>(tilesToDestroy.Union(left.Union(right.Union(up.Union(down)))));
            }
        }
        return tilesToDestroy;
    }

    private List<Tile> FindContiguous(Tile t, Vector2Int direction) {
        List<Tile> contiguous = new List<Tile>();
        contiguous.Add(t);
        Tile current = t;
        while (true) {
            Tile neighbor = GetTileAtPos(current.position + direction);
            if (neighbor != null && t.id == neighbor.id) {
                contiguous.Add(neighbor);
                current = neighbor;
            } else {
                break;
            }
        }
        return contiguous;
    }

    private Tile GetTileUnderMouse() {
        foreach (var hit in Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero)) {
            Tile t = hit.collider.GetComponent<Tile>();
            if (t != null) {
                return t;
            }
        }
        return null;
    }

    private Tile GetTileAtPos(Vector2Int position) {
        foreach (Tile t in _tiles) {
            if (t.position == position) {
                return t;
            }
        }
        return null;
    }

    private void Refill() {
        for (int columnIndex = 0; columnIndex < size.x; columnIndex++) {
            int numberInColumn = GetLowestUnoccupiedPositionBelow(new Vector2Int(columnIndex, size.y));
            int numberToSpawn = size.y - numberInColumn;
            for (int i = 0; i < numberToSpawn; i++) {
                var newTile = DoSpawn();
                newTile.position = new Vector2Int(columnIndex, size.y + i);
                newTile.transform.position = (Vector2)newTile.position;
            }
        }
        SlideDown();
    }

    private void SlideDown() {
        for (int columnIndex = 0; columnIndex < size.x; columnIndex++) {
            for (int rowIndex = 0; rowIndex < size.y * 2; rowIndex++) {
                Tile t = GetTileAtPos(new Vector2Int(columnIndex, rowIndex));
                if (t != null) {
                    t.position.y = GetLowestUnoccupiedPositionBelow(t.position);
                }
            }
        }
    }

    private int GetLowestUnoccupiedPositionBelow(Vector2Int position) {
        for (int rowIndex = 0; rowIndex < position.y; rowIndex++) {
            if (GetTileAtPos(new Vector2Int(position.x, rowIndex)) == null) {
                return rowIndex;
            }
        }
        return position.y;
    }

    private Tile DoSpawn() {
        Tile prefab = tilePrefabs[Random.Range(0, tilePrefabs.Count)];
        Tile newTile = Instantiate(prefab.gameObject, transform).GetComponent<Tile>();
        _tiles.Add(newTile);
        return newTile;
    }
}
