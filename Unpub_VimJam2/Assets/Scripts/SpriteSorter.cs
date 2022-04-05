using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    public Vector2 centerOffset;
    private SpriteRenderer[] _renderers;
    private void Start() {
        _renderers = GetComponentsInChildren<SpriteRenderer>();
    }
    private void Update() {
        foreach (var spriteRenderer in _renderers) {
            spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.TransformPoint(centerOffset).y*100);
        }
    }
    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.TransformPoint(centerOffset), 0.1f);
    }
}
