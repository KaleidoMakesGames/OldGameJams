using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class LevelBoundsCalculator : MonoBehaviour {
    public Bounds bounds {
        get {
            Tilemap levelTilemap = GetComponent<Tilemap>();
            levelTilemap.CompressBounds();
            Vector3 minimum = levelTilemap.CellToWorld(levelTilemap.origin);
            Vector3 maximum = levelTilemap.CellToWorld(levelTilemap.origin + levelTilemap.size) + levelTilemap.cellSize;

            Bounds newBounds = new Bounds();
            newBounds.SetMinMax(minimum, maximum);

            return newBounds;
        }
    }
}
