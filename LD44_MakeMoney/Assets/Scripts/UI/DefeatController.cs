using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DefeatController : MonoBehaviour
{
    [System.Serializable]
    public enum DefeatType { Broke, Fired}

    public DefeatType defeatType;

    public TextMeshProUGUI explanationText;

    public void SetDefeatType(int type) {
        string text = "";
        switch((DefeatType)type) {
            case DefeatType.Broke:
                text = "You went broke.\n\nYou need to work harder next time.";
                break;
            case DefeatType.Fired:
                text = "You were fired.\n\nYou need to work harder next time.";
                break;
            default:
                break;
        }
        explanationText.text = text;
    }
}
