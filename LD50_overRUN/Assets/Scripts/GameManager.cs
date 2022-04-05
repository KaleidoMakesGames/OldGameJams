using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [ReadOnly] public int deathsSoFar;
    [ReadOnly] public int rescued;
    [ReadOnly] public int zombiesSlain;
    public GameObject gameOverDisplayer;
    public int maxZombies;

    public AudioSource audioSource;

    public AudioClip shoot;
    public AudioClip zombieDead;
    public AudioClip playerDead;
    public AudioClip selection;
    public AudioClip alright;

    public static GameManager instance { get; private set; }

    public int numberSafety {
        get {
            return Island.islandForLocation[Boat.Location.Freedom].UnitsOnIsland().Count;
        }
    }
    public int numberInDanger {
        get {
            return Island.islandForLocation[Boat.Location.Central].UnitsOnIsland().Count;
        }
    }

    private void Awake() {
        if(instance != null) {
            Debug.LogWarning("Game manager singleton already exists. Offender: " + this.name);
        }
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsGameOver()) {
            DoGameOver();
        }
    }

    public void DoGameOver() {
        foreach(var s in FindObjectsOfType<ZombieSpawner>()) {
            s.gameObject.SetActive(false);
        }
        gameOverDisplayer.SetActive(true);
        enabled = false;
    }

    public bool IsGameOver() {
        if (numberInDanger > 0) {
            // Somebody is still on the island.
            return false;
        }

        // Nobody is left on the island. Is anybody still in the boat?
        if (Boat.instance.contents.Where(x => x.GetType() == typeof(SoldierController)).Count() == 0) {
            // Nobody is in the boat, we're all done.
            return true;
        }

        // Sombody is in the boat and they haven't reached safety yet.
        return false;
    }
}
