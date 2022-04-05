using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerChoices : Singleton<PlayerChoices> {
    public bool canInfiniJump;
    public bool canDash;
    public bool hasFullVision;
    public bool canAttack;
    public bool canSlowTime;

    public int currentLevel;

    private void Awake() {
        ResetChoices();
    }

    public void ResetChoices() {
        canInfiniJump = true;
        canDash = true;
        hasFullVision = true;
        canAttack = true;
        canSlowTime = true;
        currentLevel = 1;
    }

    public static T FindGaiosComponent<T>() where T : MonoBehaviour {
        return FindObjectOfType<GaiosChoiceApplier>().GetComponent<T>();
    }

    public void ReachedEndOfLevel() {
        SceneManager.LoadScene("Choice");
        currentLevel++;
    }

    public void FinishedChoosing() {
        SceneManager.LoadScene("Loading");
        StartCoroutine(LoadNewScene());
    }

    // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
    IEnumerator LoadNewScene() {
        yield return new WaitForSeconds(1.0f);
        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        AsyncOperation async = SceneManager.LoadSceneAsync("Level " + currentLevel);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone) {
            yield return null;
        }

    }

    public void SacrificeSelf() {
        SceneManager.LoadScene("End Game");
    }
}
