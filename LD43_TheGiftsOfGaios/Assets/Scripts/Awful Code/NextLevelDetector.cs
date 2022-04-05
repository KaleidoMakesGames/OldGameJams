using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NextLevelDetector : MonoBehaviour {
    public UnityEvent ReachedEndOfLevel;

    private void OnTriggerEnter2D(Collider2D collision) {
        NextLevelRegion region = collision.GetComponent<NextLevelRegion>();
        if(region != null) {
            ReachedEndOfLevel.Invoke();
        }
    }

    public void DoNextLevel() {
        PlayerChoices.Instance.ReachedEndOfLevel();
    }
}
