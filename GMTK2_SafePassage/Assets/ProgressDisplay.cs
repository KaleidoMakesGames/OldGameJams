using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressDisplay : MonoBehaviour
{

    public Transform groundpoint;
    public CarController c;
    public TMPro.TextMeshProUGUI textField;

    // Update is called once per frame
    void Update() {
        textField.text = "Altitude: " + (int)Mathf.Max(0, c.transform.position.y - groundpoint.transform.position.y) + "m";
    }
}
