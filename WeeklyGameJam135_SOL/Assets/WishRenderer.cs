using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WishRenderer : MonoBehaviour
{
    Camera gc;
    private RectTransform rt;

    private void Awake() {
        gc = Camera.main;
        rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        rt.anchorMax = gc.ScreenToViewportPoint(Input.mousePosition);
        rt.anchorMin = gc.ScreenToViewportPoint(Input.mousePosition);
    }
}
