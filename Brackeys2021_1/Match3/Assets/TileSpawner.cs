using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    public TileGrid tileGrid;
    public List<TileType> types;
    public Tile tilePrefab;
    public bool autoFill;

    private System.Random random;

    private void Awake() {
        random = new System.Random();
        if (autoFill) {
            tileGrid.OnScore.AddListener(Fill);
        }
    }

    private void Start() {
        Fill();
    }

    public void Fill() {
        foreach(var position in tileGrid.GetEmptySpots()) {
            var spawned = Instantiate(tilePrefab, tileGrid.transform).GetComponent<Tile>();
            spawned.position = position;
            tileGrid.AddTile(spawned);
            TryUniqueType(spawned);
        }
    }

    private void TryUniqueType(Tile tile) {
        foreach(TileType type in RandomTiles()) {
            tile.type = type;
            if(tileGrid.FindContiguous(tile, Vector2Int.right).Count < 3 &&
                tileGrid.FindContiguous(tile, Vector2Int.left).Count < 3 &&
                tileGrid.FindContiguous(tile, Vector2Int.up).Count < 3 &&
                tileGrid.FindContiguous(tile, Vector2Int.down).Count < 3) {
                return;
            }
        }

        tile.type = types[UnityEngine.Random.Range(0, types.Count)];
    }

    private List<TileType> RandomTiles() {
        int n = types.Count;

        List<TileType> copy = new List<TileType>(types);
        while(n > 1) {
            n--;
            int rnd = random.Next(n + 1);

            var value = copy[rnd];
            copy[rnd] = copy[n];
            copy[n] = value;
        }
        return copy;
    }
}

