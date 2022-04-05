using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMouseFollow : MonoBehaviour
{
    public Camera c;
    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 mp = Input.mousePosition;
        Vector3 cam = -Vector3.forward * Camera.main.transform.position.z;
        Vector3 screen = mp + cam;
        Vector3 world = c.ScreenToWorldPoint(screen);
        transform.position = world;
    }
}
