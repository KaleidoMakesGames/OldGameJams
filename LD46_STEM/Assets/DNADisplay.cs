using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNADisplay : MonoBehaviour
{
    public RectTransform rt;
    public TMPro.TextMeshProUGUI textField;

    private DNATracker tracker;

    private void Awake() {
        tracker = FindObjectOfType<DNATracker>();
    }

    private void Update() {
        textField.text = tracker.currentDNA.ToString() + "/" + tracker.numberToWin + " NUCLEOTIDES FOUND";
        rt.sizeDelta = new Vector2(50 * tracker.currentDNA, rt.sizeDelta.y);    
    }
}
