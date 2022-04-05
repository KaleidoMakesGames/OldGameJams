using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExposureAnimator : MonoBehaviour
{
    public float scale;
    public float speed;
    public float center;
    public Material m;

    public float rotSpeed;

    // Update is called once per frame
    void Update()
    {
        float exp = (Mathf.PerlinNoise(0.0f, Time.time * speed)-0.5f) * scale + center;
        m.SetFloat("_Exposure", exp);

        m.SetFloat("_Rotation", Mathf.Repeat(m.GetFloat("_Rotation") + rotSpeed * Time.deltaTime, 360.0f));
    }
}
