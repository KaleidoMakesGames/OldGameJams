using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechAnimator : MonoBehaviour
{
    public AttackController attackController;
    public Transform lArm;
    public Transform rArm;
    
    // Update is called once per frame
    void Update() {
        Vector3 lookAtPoint = attackController.fireTarget;
        lookAtPoint.y = transform.position.y;
        transform.LookAt(lookAtPoint, Vector3.up);

        Vector3 deltaL = attackController.fireTarget - lArm.position;
        deltaL.y = 0.0f;
        lArm.rotation = Quaternion.LookRotation(Vector3.down, deltaL);

        Vector3 deltaR = attackController.fireTarget - rArm.position;
        deltaR.y = 0.0f;
        rArm.rotation = Quaternion.LookRotation(Vector3.down, deltaR);
    }
}
