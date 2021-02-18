using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRenderer : MonoBehaviour
{
    public Tile tile;
    new public SpriteRenderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        renderer.color = tile.type.color;
        renderer.sprite = tile.type.image;
    }
}
