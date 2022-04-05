using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorRungRenderer : MonoBehaviour
{
    public AnimationCurve curve;

    private PlayerMovementController player;

    private void Awake() {
        player = FindObjectOfType<PlayerMovementController>();
    }
    // Update is called once per frame
    void Update()
    {
        if(player == null) {
            return;
        }
        foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>()) {
            float distance = Vector3.Distance(player.transform.position, mr.transform.position);
            float alpha = curve.Evaluate(distance);
            mr.material.color = new Color(mr.material.color.r,
                mr.material.color.g,
                mr.material.color.b,
                alpha);
        }
    }
}
