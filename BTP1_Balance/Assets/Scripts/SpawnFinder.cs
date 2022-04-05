using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFinder : MonoBehaviour {
    public void RefindPoint() {
        LevelEntryway entryway = FindObjectOfType<LevelEntryway>();
        if (entryway != null) {
            transform.position = new Vector3(entryway.transform.position.x, entryway.transform.position.y, transform.position.z);
        }
    }
}
