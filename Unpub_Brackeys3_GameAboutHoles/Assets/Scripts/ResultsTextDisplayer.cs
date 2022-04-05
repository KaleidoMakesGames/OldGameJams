using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ResultsTextDisplayer : MonoBehaviour
{
    public TMPro.TextMeshProUGUI solfield;
    public TMPro.TextMeshProUGUI totfield;
    public HistorianManager manager;
    
    // Update is called once per frame
    void Update()
    {
        solfield.text = manager.movesInSolution.ToString();
        totfield.text = manager.totalMoves.ToString();
    }
}
