using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSelectorListDisplay : MonoBehaviour
{
    public CellSelectorItemDisplay prefab;

    public CellBuilder builder;

    public Transform container;

    public TooltipController tooltipController;

    private DNATracker tracker;

    private List<CellSelectorItemDisplay> children;

    private void Awake() {
        tracker = FindObjectOfType<DNATracker>();
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateList();
    }

    private void Update() {
        foreach(CellSelectorItemDisplay i in children) {
            if(i.info.dnaLevelNeeded <= tracker.currentDNA) {
                i.gameObject.SetActive(true);
            } else {
                i.gameObject.SetActive(false);
            }
        }
    }

    public void UpdateList() {
        children = new List<CellSelectorItemDisplay>();
        foreach(Transform t in container) {
            Destroy(t.gameObject);
        }
        builder.availableCells.Sort(delegate (CellInfo a, CellInfo b) {
            return a.proteinCost.CompareTo(b.proteinCost);
        });

        foreach (CellInfo info in builder.availableCells) {
            CellSelectorItemDisplay newItem = Instantiate(prefab.gameObject, container).GetComponent<CellSelectorItemDisplay>();
            newItem.info = info;
            newItem.builder = builder;
            newItem.tooltipController = tooltipController;
            children.Add(newItem);
        }
    }
}
