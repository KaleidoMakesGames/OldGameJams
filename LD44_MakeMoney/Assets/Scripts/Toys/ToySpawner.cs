using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToySpawner : MonoBehaviour
{
    public ToyController toyPrefab;

    public ToyTypeSet currentSet;
    public float toysPerSecond;

    private float lastSpawnTime = Mathf.NegativeInfinity;

    private void OnEnable() {
        lastSpawnTime = 0.0f;
    }

    private void Update() {
        if(Time.time-lastSpawnTime >= (1 / toysPerSecond)) { 
            SpawnToyFromSet();
        }
    }

    private void SpawnToyFromSet() {
        SpawnToy(currentSet.GetRandomToyType());
    }

    public void SpawnToy(ToyType toyType) {
        lastSpawnTime = Time.time;

        if (toyType == null) {
            return;
        }
        ToyController newToy = Instantiate(toyPrefab.gameObject).GetComponent<ToyController>();
        newToy.transform.position = transform.position;
        newToy.toyType = toyType;
    }
}
