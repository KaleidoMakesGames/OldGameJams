using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[ExecuteInEditMode]
public class CellSelectorItemDisplay : MonoBehaviour
     , IPointerEnterHandler
     , IPointerExitHandler {
    public Image icon;
    public TextMeshProUGUI costField;
    public Button selectButton;

    [HideInInspector] public CellInfo info;
    [HideInInspector] public CellBuilder builder;
    [HideInInspector] public TooltipController tooltipController;

    [Range(0, 1)]  public float disabledAlpha;

    public void OnPointerEnter(PointerEventData eventData) {
        tooltipController.hoverInfo = info;
        tooltipController.currentMode = TooltipController.Mode.Build;
    }

    public void OnPointerExit(PointerEventData eventData) {
        tooltipController.hoverInfo = null;
        tooltipController.currentMode = TooltipController.Mode.Destroy;
    }

    private void Start() {
        selectButton.onClick.AddListener(delegate {
            builder.BuildCell(info);
        });
    }
    private void Update() {
        if(info != null) {
            icon.sprite = info.icon;
            selectButton.interactable = builder.HasProtein(info) && builder.HasRoom();
            costField.text = info.proteinCost.ToString();
            Color c = selectButton.targetGraphic.color;
            c.a = selectButton.interactable ? 1.0f : disabledAlpha;
            selectButton.targetGraphic.color = c;

            c = icon.color;
            c.a = selectButton.interactable ? 1.0f : disabledAlpha;
            icon.color = c;
        }
    }
}
