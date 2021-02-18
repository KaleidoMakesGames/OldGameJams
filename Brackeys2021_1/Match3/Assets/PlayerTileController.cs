using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTileController : MonoBehaviour
{
    public TileGrid tileGrid;
    public Transform indicatorA;
    public Transform hoverIndicator;

    private Vector2Int? tileA;

    // Update is called once per frame
    void Update()
    {
        Vector2Int tilePosition = tileGrid.WorldToTile(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if(tileGrid.GetTileAtPosition(tilePosition) != null && (!tileA.HasValue || tileGrid.CanSwap(tileA.Value, tilePosition))) {
            hoverIndicator.gameObject.SetActive(true);

            if (Input.GetMouseButtonDown(0)) {
                if (tileA == null) {
                    tileA = tilePosition;
                } else if (tileA != tilePosition) {
                    tileGrid.SwapTiles(tileA.Value, tilePosition);
                    tileA = null;
                }
            }
            hoverIndicator.transform.position = tileGrid.TileToWorld(tilePosition);
        } else {
            hoverIndicator.gameObject.SetActive(false);
        }
        indicatorA.gameObject.SetActive(tileA.HasValue);
        if (tileA.HasValue) {
            indicatorA.transform.position = tileGrid.TileToWorld(tileA.Value);
        }
    }
}
