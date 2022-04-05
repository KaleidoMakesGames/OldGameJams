using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ApprovalDisplayer : MonoBehaviour
{
    public Transform approvalTransform;
    public ApprovalTracker tracker;

    public float speed;

    public float minRotation;
    public float maxRotation;
    
    // Update is called once per frame
    void Update()
    {
        if (approvalTransform != null && tracker != null) {
            float rotationAmount = Mathf.Lerp(minRotation, maxRotation, Mathf.InverseLerp(0.0f, tracker.maxApproval, tracker.currentApproval));
            float currentRotation = approvalTransform.localEulerAngles.z;
            float newRotation = Mathf.MoveTowardsAngle(currentRotation, rotationAmount, speed * Time.deltaTime);
            approvalTransform.localEulerAngles = new Vector3(approvalTransform.localEulerAngles.x, approvalTransform.localEulerAngles.y, newRotation);
        }
    }
}
