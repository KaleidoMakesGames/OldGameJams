using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[ExecuteInEditMode]
public class HealthTrackerViewer : MonoBehaviour {
    public HealthTracker tracker;

    public Sprite healthySprite;
    public Sprite deadSprite;
    public Image image;

    private RectTransform rectTransform;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update() {
        if(image != null && tracker != null) {
            if(tracker.currentHealth == 0) {
                image.sprite = deadSprite;
            } else {
                image.sprite = healthySprite;
            }
        }
    }

    private void FixedUpdate() {
        if (tracker == null) {
            return;
        }
        float percentage = tracker.currentHealth / tracker.maxHealth;
        rectTransform.anchorMax = new Vector2(percentage, rectTransform.anchorMax.y);
    }
    
    public void SetTrackerFromObject(Transform t) {
        tracker = t.GetComponentInChildren<HealthTracker>();
    }
}
