using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorFollower : MonoBehaviour
{
    private void Start() {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);   
    }
}
