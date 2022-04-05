using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnable : MonoBehaviour
{
    private Dictionary<SpriteRenderer, Color> colorDict;
    public void Tint(Color c) {
        colorDict = new Dictionary<SpriteRenderer, Color>();
        foreach(SpriteRenderer r in GetComponentsInChildren<SpriteRenderer>()) {
            colorDict.Add(r, r.color);
            r.color = c;
        }
    }

    public void Untint() {
        foreach(var sr in colorDict.Keys) {
            sr.color = colorDict[sr];
        }
        colorDict.Clear();
    }
}
