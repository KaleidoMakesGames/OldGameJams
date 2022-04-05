using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EnergyBarRenderer : MonoBehaviour
{
    public RectTransform bar;
    public FishController fishController;

    // Update is called once per frame
    void Update()
    {
        float percentage = fishController.currentEnergy / fishController.maxEnergy;
        bar.anchorMax = new Vector2(bar.anchorMax.x, percentage);
    }
}
