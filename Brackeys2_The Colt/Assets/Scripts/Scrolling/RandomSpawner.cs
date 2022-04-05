using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class RandomSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;

    public float spawnFrequency;
    private Collider2D spawnCollider;

    private float lastSpawnTime;

    private void Awake() {
        spawnCollider = GetComponent<Collider2D>();
        lastSpawnTime = Mathf.NegativeInfinity;
    }
    
    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastSpawnTime >= (1.0f/spawnFrequency)) {
            SpawnObject();
        }    
    }

    private void SpawnObject() {
        GameObject newObject = Instantiate(objectToSpawn, transform);
        newObject.transform.position = Utility.RandomInBounds(spawnCollider.bounds);
        lastSpawnTime = Time.time;
    }
}
