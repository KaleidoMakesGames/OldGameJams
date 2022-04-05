using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTargeter : MonoBehaviour
{
    public float offset;

    private float height;

    private void Start() {
        height = transform.position.y;
    }

    public void Advance() {

        var s = LevelConnection.GetSortedConnections();

        float height = 0.0f;
        bool found = false;
        for(int i = 0; i < s.Count; i++) {
            if(s[i].enabled) {
                height = s[i-1].GetPointAlongCurve(1).y;
                found = true;
                break;
            }
        }
        if(!found) {
            height = s[s.Count - 1].GetPointAlongCurve(1).y;
        }
        transform.position = Vector3.up * (height + offset);
    }
}
