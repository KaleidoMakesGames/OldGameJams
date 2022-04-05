using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosestUtility {
    public static T GetClosestInList<T>(Transform a, List<T> list) where T : MonoBehaviour {
        T closest = null;
        float closestDistance = Mathf.Infinity;
        foreach (T item in list) {
            float distance = Vector2.Distance(item.transform.position, a.position);
            if (distance < closestDistance) {
                closestDistance = distance;
                closest = item;
            }
        }
        return closest;
    }
    
    public static List<T> ThresholdList<T>(Transform a, List<T> list, float distanceThreshold) where T : MonoBehaviour {
        List<T> newList = new List<T>();

        foreach (T item in list) {
            float distance = Vector2.Distance(item.transform.position, a.position);
            if (distance <= distanceThreshold) {
                newList.Add(item);
            }
        }
        return newList;
    }
}
