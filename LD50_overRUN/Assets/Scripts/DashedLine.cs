using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[ExecuteAlways]
public class DashedLine : MonoBehaviour
{
    public Color color;
    public Texture texture;
    public float scrollSpeed;
    public float scale;
    
    private LineRenderer lr {
        get {
            return GetComponent<LineRenderer>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        var material = Application.isPlaying ? lr.material : lr.sharedMaterial;
        material.shader = Shader.Find("UI/Unlit/Transparent");
        material.SetTexture("_MainTex", texture);
        material.mainTextureScale = new Vector2(scale / lr.startWidth, 1);
        material.color = color;
        if (Application.isPlaying) {
            lr.material.mainTextureOffset = Vector2.right * Mathf.Repeat(lr.material.mainTextureOffset.x + scrollSpeed * Time.deltaTime, 1);
        }
    }
}
