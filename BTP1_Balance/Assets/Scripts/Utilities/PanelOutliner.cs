using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class PanelOutliner : MonoBehaviour {
    public float thickness;
    public Color color;

    public bool top;
    public bool bottom;
    public bool left;
    public bool right;

    public enum Position { Inside, Outside, Center}
    public Position position;

    private Image _left;
    private Image _right;
    private Image _top;
    private Image _bottom;

    private RectTransform container;

    private void Awake() {
    }

    private void Reset() {
        Relink();
    }

    private void Relink() {
        foreach (Transform t in transform) {
            if(t.name.Equals("** Panel Outline")) {
                DestroyImmediate(t.gameObject);
            }
        }
        container = (new GameObject("** Panel Outline", typeof(RectTransform))).GetComponent<RectTransform>();
        container.SetParent(transform, false);
        container.SetAsFirstSibling();
        FillTransform(container);

        _left = (new GameObject("Left", typeof(Image))).GetComponent<Image>();
        _right = (new GameObject("Right", typeof(Image))).GetComponent<Image>();
        _top = (new GameObject("Top", typeof(Image))).GetComponent<Image>();
        _bottom = (new GameObject("Bottom", typeof(Image))).GetComponent<Image>();

        _left.rectTransform.SetParent(container.transform, false);
        _right.rectTransform.SetParent(container.transform, false);
        _top.rectTransform.SetParent(container.transform, false);
        _bottom.rectTransform.SetParent(container.transform, false);

        FillTransform(_left.rectTransform);
        FillTransform(_right.rectTransform);
        FillTransform(_top.rectTransform);
        FillTransform(_bottom.rectTransform);

        _left.rectTransform.anchorMin = new Vector2(0.0f, 0.0f);
        _left.rectTransform.anchorMax = new Vector2(0.0f, 1.0f);

        _right.rectTransform.anchorMin = new Vector2(1.0f, 0.0f);
        _right.rectTransform.anchorMax = new Vector2(1.0f, 1.0f);

        _top.rectTransform.anchorMin = new Vector2(0.0f, 1.0f);
        _top.rectTransform.anchorMax = new Vector2(1.0f, 1.0f);

        _bottom.rectTransform.anchorMin = new Vector2(0.0f, 0.0f);
        _bottom.rectTransform.anchorMax = new Vector2(1.0f, 0.0f);
    }

    private void Update() {
        if(_left == null) {
            Relink();
        }

        _left.color = color;
        _right.color = color;
        _top.color = color;
        _bottom.color = color;

        float leftPivot = 0.0f;
        float rightPivot = 0.0f;
        float tbExtra = 0.0f;

        switch(position) {
            case Position.Center:
                leftPivot = 0.5f;
                rightPivot = 0.5f;
                tbExtra = -thickness;
                break;
            case Position.Inside:
                leftPivot = 0.0f;
                rightPivot = 1.0f;
                tbExtra = -2 * thickness;
                break;
            case Position.Outside:
                leftPivot = 1.0f;
                rightPivot = 0.0f;
                tbExtra = 2 * thickness;
                break;
        }
        _left.rectTransform.pivot = new Vector2(leftPivot, 0.5f);
        _right.rectTransform.pivot = new Vector2(rightPivot, 0.5f);
        _top.rectTransform.pivot = new Vector2(0.5f, rightPivot);
        _bottom.rectTransform.pivot = new Vector2(0.5f, leftPivot);

        _left.rectTransform.sizeDelta = new Vector2(thickness, position == Position.Center ? thickness : 0.0f);
        _right.rectTransform.sizeDelta = new Vector2(thickness, position == Position.Center ? thickness : 0.0f);
        _top.rectTransform.sizeDelta = new Vector2(tbExtra, thickness);
        _bottom.rectTransform.sizeDelta = new Vector2(tbExtra, thickness);

        _left.gameObject.SetActive(left);
        _right.gameObject.SetActive(right);
        _top.gameObject.SetActive(top);
        _bottom.gameObject.SetActive(bottom);
    }

    private static void FillTransform(RectTransform t) {
        t.anchorMin = Vector2.zero;
        t.anchorMax = Vector2.one;
        t.offsetMax = Vector2.zero;
        t.offsetMin = Vector2.zero;
    }
}
