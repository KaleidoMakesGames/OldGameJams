using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ToyExitRenderer : MonoBehaviour
{
    public ToyProcessor processor;
    public SpriteRenderer spriteRenderer;
    
    // Update is called once per frame
    void Update()
    {
        if(processor != null && spriteRenderer != null && processor.correctToyTypes.Count > 0 && processor.correctToyTypes[0] != null) {
            spriteRenderer.color = processor.correctToyTypes[0].color;
            spriteRenderer.sprite = processor.correctToyTypes[0].sprite;
            spriteRenderer.enabled = true;
        } else if (spriteRenderer != null) {
            spriteRenderer.enabled = false;
        }
    }
}
