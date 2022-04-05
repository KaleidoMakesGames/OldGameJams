using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityToggler : MonoBehaviour
{
    public GameObject toggle;
    public GridElement e;

    private void Update() {
        if(WorldGrid.Instance.IsSupported(e.position)) {
            toggle.gameObject.SetActive(true);
        } else {
            toggle.gameObject.SetActive(false);
        }
    }
}
