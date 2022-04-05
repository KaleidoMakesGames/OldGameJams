using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ArrowPositioner : MonoBehaviour
{
    public PlayArea area;
    public float offset;
    public Transform NArrow;
    public Transform SArrow;
    public Transform EArrow;
    public Transform WArrow;

    public OnBoarder onboarder;

    // Update is called once per frame
    void Update()
    {
        bool show = !onboarder.isOnboarding || onboarder.steps[onboarder.currentStep].showMoveArrows;
        NArrow.gameObject.SetActive(show);
        SArrow.gameObject.SetActive(show);
        EArrow.gameObject.SetActive(show);
        WArrow.gameObject.SetActive(show);

        NArrow.localPosition = new Vector3(area.bounds.center.x, 0.0f, area.bounds.max.z + offset);
        SArrow.localPosition = new Vector3(area.bounds.center.x, 0.0f, area.bounds.min.z - offset);
        EArrow.localPosition = new Vector3(area.bounds.max.x+offset, 0.0f, area.bounds.center.z);
        WArrow.localPosition = new Vector3(area.bounds.min.x - offset, 0.0f, area.bounds.center.z);
    }
}
