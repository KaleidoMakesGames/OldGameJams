using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public ObstacleSpawnSet spawnSet;

    private float distanceSinceLastSpawn;

    private void Awake() {
        distanceSinceLastSpawn = 0;
    }

    private int numberOfSpawns = 0;

    private void FixedUpdate() {
        if (distanceSinceLastSpawn >= Mathf.Clamp((spawnSet.initialSpawnDistance + (numberOfSpawns * spawnSet.spawnDistanceChangePerSpawn)), spawnSet.minSpawnDistance, spawnSet.maxSpawnDistance)) {
            SpawnObstacle();
        }
        if (Runner.Instance.enabled) {
            distanceSinceLastSpawn += Runner.Instance.runningVector.magnitude * Time.fixedDeltaTime;
        }
    }

    public void SpawnObstacle() {
        if (Random.Range(0.0f, 1.0f) > Mathf.Clamp(spawnSet.initialBlankProbability + (numberOfSpawns * spawnSet.blankProbabilityChangePerSpawn), spawnSet.minBlankProbability, spawnSet.maxBlankProbability)) {
            GameObject obstacleToSpawn = spawnSet.GetObstacle();
            if (obstacleToSpawn != null) {
                GameObject newObstacle = Instantiate(obstacleToSpawn);
                newObstacle.transform.position = transform.position;
            }
        }
        distanceSinceLastSpawn = 0;
        numberOfSpawns++;
    }
}
