using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UndoRedoEnabler : MonoBehaviour
{
    public Button undo;
    public Button redo;

    public PlatformMover p;
    public HistorianManager historianManager;

    public OnBoarder onboarder;

    // Update is called once per frame
    void Update() {
        bool show = !onboarder.isOnboarding || onboarder.steps[onboarder.currentStep].showControls;
        undo.gameObject.SetActive(show);
        redo.gameObject.SetActive(show);
        undo.interactable = historianManager.isReady && !p.isMoving && historianManager.CanRewind();
        redo.interactable = historianManager.isReady && !p.isMoving && historianManager.CanForward();
    }
}
