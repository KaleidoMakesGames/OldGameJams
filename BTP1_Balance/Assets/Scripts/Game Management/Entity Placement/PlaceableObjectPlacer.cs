using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObjectPlacer : MonoBehaviour {
    private List<IPlaceableObject> objectsPlaced = new List<IPlaceableObject>();

    public void RevertAll() {
        for(int i = objectsPlaced.Count-1; i >= 0; i--) {
            if(objectsPlaced[i].Equals(null)) {
                objectsPlaced.RemoveAt(i);
            }
        }
        foreach(IPlaceableObject pObject in objectsPlaced) {
            pObject.RevertToPlacement();
        }
    }

    public void PlaceObject(GameObject o, Vector2 position, Transform parent) {
        GameObject newO = Instantiate(o, parent);
        newO.transform.position = position;

        foreach (IPlaceableObject placeableObject in newO.GetComponentsInChildren<IPlaceableObject>()) {
            objectsPlaced.Add(placeableObject);
        }
    }

    public void RemoveObject(GameObject o) {
        foreach (IPlaceableObject placeableObject in o.GetComponentsInChildren<IPlaceableObject>()) {
            objectsPlaced.Remove(placeableObject);
        }
        Destroy(o);
    }
}
