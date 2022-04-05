using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FibroblastCTSpawner : MonoBehaviour
{
    public GridElement ctPrefab;
    public float radius;
    public GridElement myGridElement;

    public List<GridElement> myCTs { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        DoSpawnCT();    
    }

    private void OnDestroy() {
        DestroyContents();
    }

    private void DoSpawnCT() {
        myCTs = new List<GridElement>();
        foreach(Vector2Int tile in GetTiles()) {
            GridElement newCT = Instantiate(ctPrefab.gameObject).GetComponent<GridElement>();
            newCT.position = tile;
            myCTs.Add(newCT);
        }
    }

    private List<Vector2Int> GetTiles() {
        List<Vector2Int> tiles = new List<Vector2Int>();
        for(int x = -(int)radius; x <= (int)radius; x++) {
            for(int y = -(int)radius; y <= (int) radius; y++) {
                Vector2Int point = new Vector2Int(x, y);
                if (point.magnitude <= radius) {
                    tiles.Add(new Vector2Int(x, y) + myGridElement.position);
                }
            }
        }
        return tiles;
    }
    
    private void DestroyContents() {
        WorldGrid.Instance.RemoveFromMap(myGridElement);
        foreach(GridElement ct in myCTs) {
            Vector2Int pos = ct.position;
            if (ct != null) {
                Destroy(ct.gameObject);
            }
        }
    }
}
