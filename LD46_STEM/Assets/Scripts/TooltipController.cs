using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipController : MonoBehaviour
{
    public CellBuilder builder;
    public CellDestroyer destroyer;
    public TMPro.TextMeshProUGUI nameField;
    public TMPro.TextMeshProUGUI descriptionField;
    public TMPro.TextMeshProUGUI costField;
    public TMPro.TextMeshProUGUI instructionsField;
    public Image instructionsButton;

    public Camera mainCamera;

    public RectTransform container;

    public enum Mode {
        Build,
        Destroy,
        Other
    }
    public Mode currentMode;
    
    [HideInInspector] public CellInfo hoverInfo;
    [HideInInspector] public Tooltippable tooltippable;

    public Color canBuildColor;
    public Color cannotBuildColor;

    private RectTransform rectTransform;

    private void Awake() {
        currentMode = Mode.Destroy;
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update() {
        rectTransform.position = Input.mousePosition;

        if (currentMode == Mode.Build) {
            container.gameObject.SetActive(hoverInfo != null);
            if (hoverInfo != null) {
                nameField.text = hoverInfo.cellName;
                descriptionField.text = hoverInfo.cellDescription;
                costField.text = "<color=red>" + hoverInfo.proteinCost.ToString() + " PROTEIN </color>";
                if (builder.HasProtein(hoverInfo)) {
                    if (builder.HasRoom()) {
                        instructionsButton.color = canBuildColor;
                        instructionsField.text = "CLICK TO PRODUCE";
                    } else {
                        instructionsButton.color = cannotBuildColor;
                        instructionsField.text = "-NOT ENOUGH ROOM-";
                    }
                } else {
                    instructionsButton.color = cannotBuildColor;
                    instructionsField.text = "-NOT ENOUGH RESOURCES-";
                }
            }
        }

        if (currentMode == Mode.Destroy) {
            container.gameObject.SetActive(hoverInfo != null);
            if (hoverInfo != null) {
                nameField.text = hoverInfo.cellName;
                descriptionField.text = hoverInfo.cellDescription;
                costField.text = "";
                if (!destroyer.InRangeToDestroy(hoverInfo)) {
                    instructionsButton.color = cannotBuildColor;
                    instructionsField.text = "TOO FAR TO DESTROY";
                } else if (destroyer.IsLastSupportingCell(hoverInfo)) {
                    instructionsButton.color = cannotBuildColor;
                    instructionsField.text = "-CANNOT DESTROY CRITICAL FIBROBLAST-";
                } else {
                    instructionsButton.color = canBuildColor;
                    instructionsField.text = "(WARNING!) CLICK TO DESTROY";
                }

                if (Input.GetMouseButtonDown(0)) {
                    destroyer.DestroyCell(hoverInfo);
                }
            }
        }

        if(currentMode == Mode.Other) {
            container.gameObject.SetActive(tooltippable != null);
            if (tooltippable != null) {
                nameField.text = tooltippable.titleString;
                descriptionField.text = tooltippable.descriptionString;
                instructionsButton.color = canBuildColor;
                instructionsField.text = tooltippable.instructionString;
                costField.text = "";
            }
        }

        // Check for hovered item
        CheckHovered();
    }

    private void CheckHovered() {
        if (currentMode == Mode.Build) {
            return;
        }

        hoverInfo = null;
        tooltippable = null;
        
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }

        Vector3 mousePosWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        foreach(GridElement e in WorldGrid.Instance.ElementsAtPosition((Vector2Int)WorldGrid.Instance.grid.WorldToCell(mousePosWorld))) {
            Tooltippable t = e.GetComponent<Tooltippable>();
            if(t != null) {
                tooltippable = t;
                currentMode = Mode.Other;
                return;
            }

            CellInfo info = e.GetComponent<CellInfo>();
            if(info != null) {
                hoverInfo = info;
                currentMode = Mode.Destroy;
                return;
            }
        }
    }
}
