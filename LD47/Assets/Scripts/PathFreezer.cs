using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFreezer : MonoBehaviour
{
    public float freezeTime;
    public PathZoneCritterDetector critterDetector;
    [ReadOnly] public float remainingFreezeTime;
    // Start is called before the first frame update
    void Start() {
        foreach (var cmc in critterDetector.crittersCaught) {
            cmc.critterMovementController.rb.simulated = false;
        }
        StartCoroutine(DoTimer());
    }

    IEnumerator DoTimer() {
        remainingFreezeTime = freezeTime;
        while(remainingFreezeTime > 0) {
            yield return null;
            remainingFreezeTime = Mathf.Clamp(remainingFreezeTime - Time.deltaTime, 0.0f, Mathf.Infinity);
        }
        foreach (var cmc in critterDetector.crittersCaught) {
            cmc.critterMovementController.rb.simulated = true;
        }
        Destroy(gameObject);
    }
}
