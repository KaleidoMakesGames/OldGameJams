using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMovementController : MonoBehaviour
{
    public Tile tile;
    public float movementSpeed;

    private void Start() {
        transform.localPosition = new Vector2(tile.position.x, Mathf.Pow(tile.position.y, 1/2) + tile.grid.height);
    }
    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Vector2.MoveTowards(transform.localPosition, tile.position, movementSpeed * Time.deltaTime);
    }
}
