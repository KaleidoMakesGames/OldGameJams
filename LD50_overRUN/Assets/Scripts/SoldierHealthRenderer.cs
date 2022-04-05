using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SoldierHealthRenderer : MonoBehaviour
{
    public RectTransform bar;
    public SoldierController soldierController;

    // Update is called once per frame
    void Update()
    {
        float percentage = soldierController.currentHealth / soldierController.maxHealth;
        bar.anchorMax = new Vector2(percentage, bar.anchorMax.y);
    }
}
 