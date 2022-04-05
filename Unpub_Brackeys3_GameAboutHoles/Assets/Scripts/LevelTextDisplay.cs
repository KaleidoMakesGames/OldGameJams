using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class LevelTextDisplay : MonoBehaviour
{
    public TMPro.TextMeshProUGUI titleField;
    public TMPro.TextMeshProUGUI numberField;
    public LevelLoader levelLoader;

    public OnBoarder onboarder;

    // Update is called once per frame
    void Update()
    {
        bool show = !onboarder.isOnboarding || onboarder.steps[onboarder.currentStep].showControls;

        titleField.gameObject.SetActive(show);
        numberField.gameObject.SetActive(show);

        titleField.text = levelLoader.levels[levelLoader.currentLevel].levelTitle;
        numberField.text = "(LEVEL " + (levelLoader.currentLevel + 1) + "/" + levelLoader.levels.Count + ")";
    }
}
