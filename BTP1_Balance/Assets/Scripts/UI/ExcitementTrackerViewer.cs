using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class ExcitementTrackerViewer : MonoBehaviour {
    public ExcitementTracker tracker;

    private RectTransform rectTransform;

    public Sprite happySprite;
    public Sprite mediumSprite;
    public Sprite sadSprite;

    public Image image;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update() {
        if (tracker == null) {
            return;
        }
        if (tracker.currentExcitement / tracker.maxExcitement >= .75) {
            image.sprite = happySprite;
        } else if (tracker.currentExcitement / tracker.maxExcitement < 0.75f && tracker.currentExcitement / tracker.maxExcitement >= 0.25f) {
            image.sprite = mediumSprite;
        } else {
            image.sprite = sadSprite;
        }

    }

    private void FixedUpdate() {
        if (tracker == null) {
            return;
        }
        float percentage = tracker.currentExcitement / tracker.maxExcitement;
        rectTransform.anchorMax = new Vector2(percentage, rectTransform.anchorMax.y);
    }

    public void SetTrackerFromObject(Transform t) {
        tracker = t.GetComponentInChildren<ExcitementTracker>();
    }
}
