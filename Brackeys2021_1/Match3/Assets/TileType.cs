using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TileType : ScriptableObject {
    public Color color;
    public Sprite image;

    private void Awake() {
        color = Random.ColorHSV(0, 1, 1, 1, 1, 1);
    }
}
