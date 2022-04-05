using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {
    private Scene? lastLevel;

    public UnityEvent OnLevelLoaded;

    public string startLevel;

    private void Awake() {
        lastLevel = null;
    }

    private void Start() {
        LoadLevel(startLevel);
    }

    public void LoadLevel(string levelName) {
        if(lastLevel.HasValue) {
            SceneManager.UnloadSceneAsync(lastLevel.Value);
        }
        SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode) {
        lastLevel = scene;
        foreach(GameObject o in GameObject.FindGameObjectsWithTag("DestroyOnLoad")) {
            Destroy(o);
        }
        OnLevelLoaded.Invoke();
    }
}
