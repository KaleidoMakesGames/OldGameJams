using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionReverter : MonoBehaviour, IPlaceableObject {
    private Vector2 initialPosition;

    private void Start() {
        initialPosition = transform.position;
    }

    public void RevertToPlacement() {
        gameObject.SetActive(true);
        transform.position = initialPosition;
    }
}
