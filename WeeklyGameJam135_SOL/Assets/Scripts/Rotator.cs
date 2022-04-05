using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {
    public Vector3 spinRate;

    private void Update() {
        transform.Rotate(spinRate * Time.deltaTime, Space.Self);
    }
}
