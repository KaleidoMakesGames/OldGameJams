using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class WorldGrid : MonoBehaviour
{
    public static WorldGrid Instance;

    public Grid grid { get; private set; }

    private Dictionary<Vector2Int, List<GridElement>> elementsByPosition;

    public Bounds worldBounds {
        get {
            Bounds newB = new Bounds(Vector3.zero, Vector3.zero);
            foreach(Vector2Int pos in elementsByPosition.Keys) {
                foreach (GridElement e in elementsByPosition[pos]) {
                    if (e.GetComponent<ConnectiveTissue>() != null) {
                        newB.Encapsulate(grid.GetCellCenterWorld((Vector3Int)pos));
                    }
                }
            }
            return newB;
        }
    }

    private void Awake() {
        if(Instance != null) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
        grid = GetComponent<Grid>();
        elementsByPosition = new Dictionary<Vector2Int, List<GridElement>>();
    }

    public void AddToMap(GridElement element) {
        if (!elementsByPosition.ContainsKey(element.position)) {
            elementsByPosition.Add(element.position, new List<GridElement>());
        }
        if (!elementsByPosition[element.position].Contains(element)) {
            elementsByPosition[element.position].Add(element);
        }
    }

    public void RemoveFromMap(GridElement element) {
        if(elementsByPosition.ContainsKey(element.position) && elementsByPosition[element.position].Contains(element)) {
            elementsByPosition[element.position].Remove(element);
        }
    }

    public List<GridElement> ElementsAtPosition(Vector2Int position) {
        if (elementsByPosition.ContainsKey(position)) {
            return elementsByPosition[position];
        } else {
            return new List<GridElement>();
        }
    }
    
    public bool IsSupported(Vector2Int position) {
        foreach (GridElement e in Instance.ElementsAtPosition(position)) {
            if (e.isSupport) {
                return true;
            }
        }
        return false;
    }

    public bool CanOccupy(Vector2Int position) {
        bool isSupported = false;
        foreach (GridElement e in Instance.ElementsAtPosition(position)) {
            if (e.isObstacle) {
                return false;
            }
            if(e.isSupport) {
                isSupported = true;
            }
        }
        return isSupported;
    }
}
