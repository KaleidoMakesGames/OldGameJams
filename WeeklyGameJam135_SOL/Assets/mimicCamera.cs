using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class mimicCamera : MonoBehaviour
{
    public Camera other;
    public Camera me;

    // Update is called once per frame
    void Update()
    {
        me.transform.position = other.transform.position;
        me.transform.localEulerAngles = other.transform.localEulerAngles;
        me.transform.localScale = other.transform.localScale;
        me.fieldOfView = other.fieldOfView;
        me.farClipPlane = other.farClipPlane;
        me.nearClipPlane = other.nearClipPlane;
    }
}
