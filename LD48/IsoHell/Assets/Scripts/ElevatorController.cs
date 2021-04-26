using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ElevatorController : MonoBehaviour
{
    public Animator doors;

    public List<string> levels;
    public int currentLevel;

    public string victoryLevel;

    public BoxCollider elevatorBox;

    public UnityEvent OnLastLevel;

    public Rigidbody rb;

    public bool isInElevator {
        get {
            foreach(Collider c in Physics.OverlapBox(transform.TransformPoint(elevatorBox.center), elevatorBox.size/2, transform.rotation)) {
                if(c.GetComponentInParent<PlayerMovementController>() != null) {
                    return true;
                }
            }
            return false;
        }
    }
    public bool canUseElevator { get; private set; }

    public UnityEvent OnElevatorUse;
    public UnityEvent OnElevatorArrive;
    public UnityEvent OnScientistsFound;



    public float dropHeight;
    public float deleteAfterProgress;
    public AnimationCurve elevatorCurve;
    public float elevatorTime;
    public int numRungs;

    public float elevatorArrivalProgress { get; private set; }
    public bool isMoving;

    public Transform rungPrefab;

    private Transform oldLevel;
    private Transform newLevel;

    private void Start() {
        StartCoroutine(LoadLevel());
        canUseElevator = true;
    }
    
    // Update is called once per frame
    void Update()
    {
        if(canUseElevator && isInElevator && Input.GetKeyDown(KeyCode.E)) {
            UseElevator();
        }
        doors.SetBool("Lowered", !isMoving);
    }

    public void SetCanUseElevator() {
        canUseElevator = true;
    }

    public void UseElevator() {
        currentLevel++;
        StartCoroutine(LoadLevel());
        OnElevatorUse.Invoke();
    }

    public IEnumerator LoadLevel() {
        canUseElevator = false;
        string name;
        if (currentLevel >= levels.Count) {
            currentLevel = -1;
            name = victoryLevel;
            OnLastLevel.Invoke();
        } else {
            name = levels[currentLevel];
        }
        var asyncLoad = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone) {
            yield return null;
        }
        oldLevel = newLevel;
        var loadedScene = SceneManager.GetSceneByName(name);
        newLevel = new GameObject("Level " + name).transform;
        foreach(var root in loadedScene.GetRootGameObjects()) {
            root.transform.SetParent(newLevel, true);
            // Move everything to the center
            root.transform.position += Vector3.down * dropHeight * currentLevel;
        }

        foreach (NavMeshSurface s in FindObjectsOfType<NavMeshSurface>()) {
            var async = s.BuildNavMeshAsync();
            while(!async.isDone) {
                yield return null;
            }
        }

        SceneManager.UnloadSceneAsync(loadedScene);


        if (currentLevel == -1) {
            // Get us back home// Spawn rungs
            float spacing = dropHeight / numRungs;
            float rungStart = -dropHeight * (levels.Count - 1 - 0.5f);
            int nRungs = Mathf.FloorToInt(numRungs * (levels.Count - 0.5f)) + 1;
            for (float i = 0; i < nRungs; i++) {
                var newRung = Instantiate(rungPrefab.gameObject, newLevel);
                newRung.transform.position = Vector3.up * (spacing * i + rungStart);
            }
        } else {
            // Spawn rungs
            float spacing = dropHeight / numRungs;
            float rungStart = dropHeight * (currentLevel - 0.5f);
            for (float i = 0; i < numRungs; i++) {
                var newRung = Instantiate(rungPrefab.gameObject, newLevel);
                newRung.transform.position = Vector3.down * (spacing * i + rungStart);
            }
        }
        OnLevelLoaded();
    }

    public void OnLevelLoaded() {
        if(Mathf.Approximately(transform.position.y, dropHeight*currentLevel)) {
            if(oldLevel != null) {
                Destroy(oldLevel.gameObject);
            }
            OnElevatorArrive.Invoke();
        } else {
            StartCoroutine(DoDrop());
        }
    }

    public IEnumerator DoDrop() {
        Vector3 goalPosition = Vector3.down * dropHeight * currentLevel;
        Vector3 startPosition = transform.position;
        float floorsToMove = Mathf.Abs(goalPosition.y - startPosition.y) / dropHeight;
        float time = 0.0f;
        float effectiveTime = (elevatorTime * floorsToMove);
        elevatorArrivalProgress = 0.0f;
        isMoving = true;
        bool deleted = false;
        while(time <= effectiveTime) {
            elevatorArrivalProgress = time / effectiveTime;
            if(!deleted && elevatorArrivalProgress >= deleteAfterProgress) {
                if (oldLevel != null) {
                    Destroy(oldLevel.gameObject);
                }
                deleted = true;
            }
            rb.MovePosition(Vector3.Lerp(startPosition, goalPosition, elevatorCurve.Evaluate(elevatorArrivalProgress)));
            yield return new WaitForFixedUpdate();
            time += Time.fixedDeltaTime;
        }

        elevatorArrivalProgress = 1.0f;
        rb.MovePosition(goalPosition);
        isMoving = false;
        OnElevatorArrive.Invoke();
    }
}
