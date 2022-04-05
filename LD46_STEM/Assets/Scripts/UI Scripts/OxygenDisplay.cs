using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class OxygenDisplay : MonoBehaviour
{
    public OxygenTracker oxygenTracker;
    public RectTransform container;
    public Image bar;

    private void Update() {
        if (oxygenTracker == null) {
            return;
        }

        container.sizeDelta = new Vector2(oxygenTracker.maxOxygen, container.sizeDelta.y);
        bar.rectTransform.anchorMax = new Vector2(oxygenTracker.currentOxygen / oxygenTracker.maxOxygen, bar.rectTransform.anchorMax.y);
    }
}
