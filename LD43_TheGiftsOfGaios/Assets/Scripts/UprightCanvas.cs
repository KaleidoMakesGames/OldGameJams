using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UprightCanvas : MonoBehaviour {
    private void Update() {
        transform.up = Vector2.up;
        transform.position = transform.parent.position + Vector3.up * (Vector2.Distance(transform.parent.position, transform.position));
        transform.right = Vector2.right;
        if(transform.parent.localScale.x < 0) {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        } else {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }
}
