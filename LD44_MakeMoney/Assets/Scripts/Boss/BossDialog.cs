using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Make Money/Boss Dialog")]
public class BossDialog : ScriptableObject
{
    [TextArea] public string text;
    public float timeToShow;
}
