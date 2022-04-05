using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSpawner : MonoBehaviour {
    [System.Serializable]
    public class PlayerEvent : UnityEvent<Transform> { }

    public UnityEvent OnSpawnedPlayerEnd;
    public UnityEvent OnSpawnedPlayerDied;
    public UnityEvent OnSpawnedPlayerExcitementLost;

    public DoorWalker.DoorEvent OnPlayerEnteredDoor;

    public Transform playerToSpawn;

    public PlayerEvent OnPlayerSpawned;

    private List<GameObject> spawnedPlayers = new List<GameObject>();

    public void SpawnPlayer() {
        GameObject newPlayer = Instantiate(playerToSpawn.gameObject);
        newPlayer.transform.position = transform.position;

        spawnedPlayers.Add(newPlayer);

        OnPlayerSpawned.Invoke(newPlayer.transform);

        newPlayer.GetComponent<HealthTracker>().OnDied.AddListener(delegate {
            OnSpawnedPlayerEnd.Invoke();
            OnSpawnedPlayerDied.Invoke();
        });

        newPlayer.GetComponent<ExcitementTracker>().OnLostExcitement.AddListener(delegate {
            OnSpawnedPlayerEnd.Invoke();
            OnSpawnedPlayerExcitementLost.Invoke();
        });
        
        newPlayer.GetComponent<DoorWalker>().OnEnteredDoor.AddListener(delegate (string doorway) {
            OnPlayerEnteredDoor.Invoke(doorway);
        });
    }

    public void RefindPoint() {
        LevelEntryway entryway = FindObjectOfType<LevelEntryway>();
        if(entryway != null) {
            transform.position = entryway.transform.position;
        }
    }

    public void DestroyAllPlayers() {
        foreach(GameObject player in spawnedPlayers) {
            Destroy(player);
        }
        spawnedPlayers.Clear();
    }

    public void InactiveDestroyAll() {
        StartCoroutine(DestroyAllUtil());
    }

    private IEnumerator DestroyAllUtil() {
        foreach (GameObject player in spawnedPlayers) {
            player.SetActive(false);
        }
        yield return new WaitForSeconds(.1f);
        DestroyAllPlayers();
    }
}
