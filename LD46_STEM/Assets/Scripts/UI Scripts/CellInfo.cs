using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellInfo : MonoBehaviour
{
    public int proteinCost;
    public string cellName;
    public string cellDescription;
    public Sprite icon;
    public int dnaLevelNeeded;

    public GridElement gridElement { get; private set; }

    private void Awake() {
        gridElement = GetComponent<GridElement>();
    }
}
