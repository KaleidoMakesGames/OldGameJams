using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ArenaRenderer : MonoBehaviour
{
    public Transform inner;
    public Transform outer;
    public float radius;
    public float thickness;

    // Update is called once per frame
    void Update()
    {
        inner.localScale = Vector2.one * (radius * 2 - thickness);
        outer.localScale = Vector2.one * (radius * 2 + thickness);
    }
}
