using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HeightDisplayer : MonoBehaviour
{
    public CometController c;
    public OrbitBody star;
    public RectTransform rt;

    public EscapeRingRenderer r;
    
    // Update is called once per frame
    void Update()
    {
        float current = Vector2.Distance(c.transform.position, star.transform.position);
        float percentage = Mathf.Clamp01((current - star.radius) / (r.heightToEscape - star.radius));
        rt.anchorMax = new Vector2(rt.anchorMax.x, percentage);
    }
}
