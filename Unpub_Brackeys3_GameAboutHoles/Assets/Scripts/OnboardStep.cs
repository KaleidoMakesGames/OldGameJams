using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class OnboardStep : ScriptableObject
{
    [System.Serializable]
    public enum AdvanceAction { ClickToContinue, MovePlatform, ClickOrMove}
    [System.Serializable]
    public enum TypeToHighlight { None, Pushbodies, Holes, Bombs}
    public string instructionText;
    public AdvanceAction advanceAction;
    public TypeToHighlight highlightObjects;
    public Vector2 textPosition;
    public bool showControls;
    public bool showMoveArrows;
    public bool alignCenter;
}
