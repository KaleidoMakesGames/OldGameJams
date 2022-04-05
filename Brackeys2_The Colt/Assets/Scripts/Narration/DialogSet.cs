using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Colt/Dialogue Set")]
public class DialogSet : ScriptableObject
{
    [System.Serializable]
    public struct Dialogue {
        [TextArea] public string text;
    }

    public List<Dialogue> lines;
}
