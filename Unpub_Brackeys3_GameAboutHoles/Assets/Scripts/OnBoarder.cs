using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[ExecuteInEditMode]
public class OnBoarder : MonoBehaviour
{
    public ArrowIndicatorController controller;

    public TMPro.TextMeshProUGUI instructionsField;
    public Button advanceButton;

    public ArrowPositioner p;

    public List<OnboardStep> steps;

    public PlatformMover mover;

    public RectTransform tutorialPanel;

    public LevelLoader loader;
    public OnboardStep level3Step;

    public float moveSpeed = 100.0f;

    public int currentStep;

    public bool isOnboarding {
        get {
            return currentStep >= 0 && currentStep < steps.Count;
        }
    }

    private void Awake() {
        currentStep = -1;
    }

    private void Start() {
        NextStep();
    }

    private void Update() {
        if (currentStep >= steps.Count || currentStep < 0) {
            tutorialPanel.gameObject.SetActive(false);
            if(loader.currentLevel == 2) {

            }
        } else {
            tutorialPanel.gameObject.SetActive(true);
            advanceButton.gameObject.SetActive(steps[currentStep].advanceAction == OnboardStep.AdvanceAction.ClickToContinue || 
                steps[currentStep].advanceAction == OnboardStep.AdvanceAction.ClickOrMove);
            instructionsField.text = steps[currentStep].instructionText;
            instructionsField.alignment = steps[currentStep].alignCenter ? TMPro.TextAlignmentOptions.Center : TMPro.TextAlignmentOptions.Justified;
            tutorialPanel.anchoredPosition = Vector2.MoveTowards(tutorialPanel.anchoredPosition, steps[currentStep].textPosition, 
                Vector2.Distance(tutorialPanel.anchoredPosition, steps[currentStep].textPosition) * moveSpeed * Time.deltaTime);
        }
    }

    public void NextStep() {
        if (!enabled)
            return;

        currentStep++;

        if(!isOnboarding) {
            return;
        }

        instructionsField.text = steps[currentStep].instructionText;
        controller.ClearArrows();
        List<Transform> o = new List<Transform>();
        switch (steps[currentStep].highlightObjects) {
            case OnboardStep.TypeToHighlight.Bombs:
                foreach (BombController body in FindObjectsOfType<BombController>()) {
                    o.Add(body.transform);
                }
                break;
            case OnboardStep.TypeToHighlight.Holes:
                foreach (HoleDetector body in FindObjectsOfType<HoleDetector>()) {
                    o.Add(body.transform);
                }
                break;
            case OnboardStep.TypeToHighlight.Pushbodies:
                foreach(PushBody body in FindObjectsOfType<PushBody>()) {
                    o.Add(body.transform);
                }
                break;
            case OnboardStep.TypeToHighlight.None:
                break;
        }
        controller.IndicateObjects(o);

        mover.OnMove.RemoveAllListeners();
        if (steps[currentStep].advanceAction == OnboardStep.AdvanceAction.MovePlatform ||
            steps[currentStep].advanceAction == OnboardStep.AdvanceAction.ClickOrMove) {
            mover.OnMove.AddListener(delegate {
                NextStep();
            });
        }
    }
}
