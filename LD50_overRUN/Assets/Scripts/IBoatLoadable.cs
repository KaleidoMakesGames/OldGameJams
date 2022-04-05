using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBoatLoadable
{
    public void Load();
    public void Unload(Vector2 position);
    public Sprite GetSprite();
    public Color GetColor();
}
