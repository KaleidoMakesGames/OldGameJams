using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="BALANCE/Dialogue")]
public class Dialogue : ScriptableObject {
    [System.Serializable]
    public struct DialogueBlock {
        [TextArea] public string text;
        public float secondsToDisplay;
    }

    public List<DialogueBlock> dialogue;
}
