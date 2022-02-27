using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ArrowRenderer : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    public float width;
    public float length;

    private void Reset() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        spriteRenderer.size = new Vector2(width, length);
        spriteRenderer.transform.localPosition = Vector2.up * length / 2;
        if (Application.isPlaying) {
            spriteRenderer.material.SetFloat("_LineLength", length);
            spriteRenderer.material.SetFloat("_LineWidth", width);
        } else {
            spriteRenderer.sharedMaterial.SetFloat("_LineLength", length);
            spriteRenderer.sharedMaterial.SetFloat("_LineWidth", width);
        }
    }
}
