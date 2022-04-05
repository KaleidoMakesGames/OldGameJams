using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxMover : MonoBehaviour
{
    [Range(0, 1)] public float strength;
    public SpriteRenderer spriteRenderer;
    private Camera _camera;
    private Material material;
    private Vector2 offset;
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        _lastCameraPosition = _camera.transform.position;
        material = spriteRenderer.material;
    }

    private Vector2 _lastCameraPosition;
    // Update is called once per frame
    void Update()
    {
        Vector2 delta = (Vector2)_camera.transform.position - _lastCameraPosition;
        offset -= Vector2.Lerp(Vector2.zero, delta, strength);
        material.SetVector(Shader.PropertyToID("_Offset"), offset);
        _lastCameraPosition = _camera.transform.position;
    }
}
