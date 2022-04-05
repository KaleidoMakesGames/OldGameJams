using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowIndicatorController : MonoBehaviour
{
    public GameObject arrowIndicator;

    private List<GameObject> arrows;

    private void Awake() {
        arrows = new List<GameObject>();
    }

    public void ClearArrows() {
        if(arrows == null) {
            return;
        }
        foreach(GameObject o in arrows) {
            Destroy(o);
        }
        arrows.Clear();
    }
    public void IndicateObjects(List<Transform> objects) {
        foreach(Transform t in objects) {
            GameObject newArrow = Instantiate(arrowIndicator, t);
            newArrow.transform.position = t.transform.position + Vector3.one * 0.5f + Vector3.up*0.7f;
            newArrow.transform.up = Vector3.up;
            arrows.Add(newArrow);
        }
    }
}
