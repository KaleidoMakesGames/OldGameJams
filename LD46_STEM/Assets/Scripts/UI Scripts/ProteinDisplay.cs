using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProteinDisplay : MonoBehaviour
{
    public ProteinTracker proteinTracker;
    public RectTransform container;
    public Image bar;

    private void Update() {
        if (proteinTracker == null) {
            return;
        }

        if (proteinTracker.maxProtein == -1) {
            container.sizeDelta = new Vector2(proteinTracker.currentProtein, container.sizeDelta.y);
            bar.rectTransform.anchorMax = new Vector2(1.0f, bar.rectTransform.anchorMax.y);
        } else {
            container.sizeDelta = new Vector2(proteinTracker.maxProtein, container.sizeDelta.y);
            bar.rectTransform.anchorMax = new Vector2(proteinTracker.currentProtein/proteinTracker.maxProtein, bar.rectTransform.anchorMax.y);
        }
    }
}
