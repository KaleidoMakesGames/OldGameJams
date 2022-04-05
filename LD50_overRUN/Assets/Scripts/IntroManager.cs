using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour {
    public static IntroManager instance ;
    public GameObject introCamera;
    public GameObject zoomToBoatCamera;
    public GameObject boatFollowCamera;
    public GameObject mainCamera;

    public TMPro.TextMeshProUGUI dialogueTextPanel;
    public GameObject dialogueContainer;
    public GameObject mainGameUI;

    public bool skip;

    private enum Stage { WAIT_FOR_SELECTION, WAIT_FOR_MOVE, WAIT_FOR_DEPART, WAIT_FOR_ARRIVAL, WAIT_FOR_UNLOAD, DONE}
    Stage stage = Stage.WAIT_FOR_SELECTION;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance != null) {
            Debug.LogWarning("Intro manager singleton already exists. Offender: " + this.name);
        }
        instance = this;

        if(skip) {
            stage = Stage.DONE;
            StartCoroutine(DoneTextTemporary());
            return;
        }

        foreach(var spawner in FindObjectsOfType<ZombieSpawner>()) {
            spawner.enabled = false;
        }

        introCamera.SetActive(true);
        zoomToBoatCamera.SetActive(false);
        boatFollowCamera.SetActive(false);
        mainCamera.SetActive(false);
        mainGameUI.SetActive(false);
        dialogueContainer.SetActive(true);
        dialogueTextPanel.text = "There's a <color=red>zombie</color> infestation on the mainland! Your troops are needed.\nClick on a <color=#AAFFAA>soldier</color> to command.";
    }

    public void OnSelectionMade() {
        if (stage == Stage.WAIT_FOR_SELECTION) {
            dialogueTextPanel.text = "<b>Right</b>-click to tell them where to go!";
            stage = Stage.WAIT_FOR_MOVE;
        }
    }

    public void OnMoveCommandSent() {
        if (stage == Stage.WAIT_FOR_MOVE) {
            zoomToBoatCamera.SetActive(true);
            dialogueTextPanel.text = "Load up the boat with your troops! Four can fit at a time.\nClick <b>DEPART</b> when you're ready to save the day.";
            stage = Stage.WAIT_FOR_DEPART;
        }
    }

    public void OnDepart() {
        if(stage == Stage.WAIT_FOR_DEPART) {
            stage = Stage.WAIT_FOR_ARRIVAL;
            boatFollowCamera.SetActive(true);
            dialogueTextPanel.text = "Time to save the city.\nLoad this boat up with <color=#AAAAFF>citizens</color> and bring them all to safety.";
        }
    }

    public void OnArrival() {
        if(stage == Stage.WAIT_FOR_ARRIVAL) {
            stage = Stage.WAIT_FOR_UNLOAD;
            mainCamera.SetActive(true);
            dialogueTextPanel.text = "Get your troops on the ground! The <color=red>zombies</color> are on their way...";
        }
    }

    public void OnUnload() {
        if(stage == Stage.WAIT_FOR_UNLOAD) {
            stage = Stage.DONE;
            StartCoroutine(DoneTextTemporary());
        }
    }

    public IEnumerator DoneTextTemporary() {
        FindObjectOfType<SoldierSpawner>().enabled = true;
        dialogueTextPanel.text = "Spread out! You can bring more <color=#AAFFAA>soldiers</color> to the island, but\n<b><color=red>keep casualties to a minimum!</color></b>";
        yield return new WaitForSeconds(5.0f);
        dialogueContainer.SetActive(false);
        mainGameUI.SetActive(true);
        foreach (var zombieSpawner in Resources.FindObjectsOfTypeAll<ZombieSpawner>()) {
            zombieSpawner.enabled = true;
            zombieSpawner.gameObject.SetActive(true);
        }
        foreach(var zombie in FindObjectsOfType<ZombieController>()) {
            zombie.isIdle = false;
        }
    }
}
