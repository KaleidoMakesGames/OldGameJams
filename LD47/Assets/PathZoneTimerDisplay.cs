using PolyLabel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PathZoneTimerDisplay : MonoBehaviour
{
    public PathFreezer freezer;
    public float pulseTime;
    public AnimationCurve pulseCurve;
    public Image dialImage;

    float lastPulse = -1;

    private void Start() {
        lastPulse = Mathf.Infinity;

        var bestCell = PolyLabel.PolyLabel.GetPolyLabel().GetPolyLabel(freezer.critterDetector.polygonCollider.points.ToList());
        if(bestCell == null) {
            return;
        }
        transform.position = freezer.critterDetector.polygonCollider.transform.TransformPoint(new Vector2(bestCell.x, bestCell.y));
        transform.localScale = Vector3.one * bestCell.d;
    }

    // Update is called once per frame
    void Update()
    {
        dialImage.fillAmount = freezer.remainingFreezeTime / freezer.freezeTime;

        float currentPulse = Mathf.Ceil(freezer.remainingFreezeTime);
        if(currentPulse < lastPulse) {
            StartCoroutine(DoPulse());
            lastPulse = currentPulse;
        }
    }

    IEnumerator DoPulse() {
        float currentTime = 0.0f;
        Vector3 localScale = transform.localScale;
        while(currentTime <= pulseTime) {
            float progress = currentTime / pulseTime;
            transform.localScale = localScale * pulseCurve.Evaluate(progress);
            yield return null;
            currentTime += Time.deltaTime;
        }
    }
}
