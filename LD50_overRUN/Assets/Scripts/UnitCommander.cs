using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitCommander : MonoBehaviour
{
    public Transform selectionIndicator;
    public Transform hoverIndicator;
    public float selectionRadius;
    public LineRenderer pathingLine;
    public LineRenderer boardingBoatLine;
    public SoldierController selectedUnit;
    public CommandIndicator commandIndicator;

    private BuildingController buildingUnderCursor;
    private Vector2 commandPosition;

    [System.Serializable]
    public enum CommandType { MOVE, BOARD, ENTER }
    private void Update() {
        UpdateSelection();
        UpdateCommands();
        CommandSelection();
        DrawPathingLines();
    }

    void UpdateCommands() {
        commandIndicator.gameObject.SetActive(selectedUnit != null);
        buildingUnderCursor = null;
        commandIndicator.mode = CommandType.MOVE;
        if(selectedUnit != null) {
            commandPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var boatInRange = Physics2D.OverlapCircleAll(commandPosition, selectionRadius).FirstOrDefault(x => x.GetComponentInParent<Boat>() != null);
            var buildingInRange = Physics2D.OverlapCircleAll(commandPosition, selectionRadius).FirstOrDefault(x => !x.isTrigger && x.GetComponentInParent<BuildingController>() != null);
            buildingUnderCursor = buildingInRange == null ? null : buildingInRange.GetComponentInParent<BuildingController>();

            if (boatInRange && Boat.instance.contents.Count < Boat.instance.capacity) {
                commandIndicator.mode = CommandType.BOARD;
                commandIndicator.transform.position = boatInRange.transform.position;
                commandPosition = boatInRange.transform.position;
            } else if (buildingUnderCursor != null && buildingUnderCursor.unitOnRoof == null) {
                commandIndicator.mode = CommandType.ENTER;
                commandIndicator.transform.position = buildingUnderCursor.roofSpot.position;
                commandPosition = buildingUnderCursor.groundSpot.position;
            } else { 
                commandIndicator.mode = CommandType.MOVE;
                commandIndicator.transform.position = commandPosition;
            }
        }
    }

    void CommandSelection() {
        if(Input.GetMouseButtonDown(1)) {
            if(selectedUnit != null) {

                GameManager.instance.audioSource.PlayOneShot(GameManager.instance.selection);
                selectedUnit.tryToBoard = false;
                selectedUnit.buildingToTryToEnter = null;
                if (commandIndicator.mode == CommandType.BOARD) {
                    commandPosition = DockZone.zoneForLocation[Island.AtPoint(selectedUnit.transform.position).location].GetRandomPoint();
                    selectedUnit.tryToBoard = true;
                } else if(commandIndicator.mode == CommandType.ENTER) {
                    selectedUnit.buildingToTryToEnter = buildingUnderCursor;
                }
                selectedUnit.MoveTo(commandPosition);
                IntroManager.instance.OnMoveCommandSent();
            }
        }
    }

    void DrawPathingLines() {
        pathingLine.enabled = false;
        boardingBoatLine.enabled = false;
        if (selectedUnit != null) {
            if (selectedUnit.agent.isMoving && !selectedUnit.animating) {
                pathingLine.enabled = true;
                pathingLine.SetPosition(0, selectedUnit.transform.position);
                pathingLine.SetPosition(1, selectedUnit.agent.waypoints.Last());
            }

            if (selectedUnit.tryToBoard) {
                boardingBoatLine.enabled = true;
                var boardPosition = selectedUnit.agent.isMoving ? selectedUnit.agent.waypoints.Last() : (Vector2)selectedUnit.transform.position;
                boardingBoatLine.SetPosition(0, boardPosition);
                boardingBoatLine.SetPosition(1, Boat.instance.transform.position);
            }
        }
    }

    // Update is called once per frame
    void UpdateSelection()
    {
        if(selectedUnit != null && !selectedUnit.gameObject.activeInHierarchy) {
            selectedUnit = null;
        }

        var hoverUnit = FindHoverUnit();
        if(hoverUnit == selectedUnit) {
            hoverUnit = null;
        }
        hoverIndicator.gameObject.SetActive(hoverUnit != null);
        if(hoverUnit != null) {
            hoverIndicator.position = hoverUnit.transform.position;
        }

        if(Input.GetMouseButtonDown(0)) {
            selectedUnit = hoverUnit;
            if(selectedUnit != null) {
                GameManager.instance.audioSource.PlayOneShot(GameManager.instance.selection);
                IntroManager.instance.OnSelectionMade();
            }
        }

        selectionIndicator.gameObject.SetActive(selectedUnit != null);
        if (selectedUnit != null) {
            selectionIndicator.position = selectedUnit.transform.position;
        }
    }

    SoldierController FindHoverUnit() {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var itemsUnderMouse = Physics2D.OverlapCircleAll(mousePosition, selectionRadius).
            Select(x => x.GetComponentInParent<SoldierController>()).
            Where(x => x != null && x.enabled && !x.animating).
            OrderBy(x => Vector2.Distance(mousePosition, x.transform.position));
        return itemsUnderMouse.FirstOrDefault();
    }
}
