using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathCritterCatcher : MonoBehaviour
{
    public PathZoneCritterDetector critterDetector;
    public AnimationCurve scaleOverShrinkProgress;
    public float shrinkTime;

    private void Start() {
        foreach (var cmc in critterDetector.crittersCaught) {
            cmc.transform.SetParent(transform);
            cmc.critterMovementController.rb.simulated = false;
        }
        StartCoroutine(DoCatch());
    }

    IEnumerator DoCatch() {
        int badCaught = critterDetector.crittersCaught.Count(v => v.isBad);

        FindObjectOfType<StrikesCounter>().RegisterStrikes(badCaught);

        float time = 0.0f;
        while (time <= shrinkTime) {
            transform.localScale = scaleOverShrinkProgress.Evaluate(time / shrinkTime) * Vector3.one;
            yield return null;
            time += Time.deltaTime;
        }
        Destroy(gameObject);
    }
}