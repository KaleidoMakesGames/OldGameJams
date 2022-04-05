using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Colt/Spawn Set")]
public class ObstacleSpawnSet : ScriptableObject
{
    public float initialBlankProbability;
    public float blankProbabilityChangePerSpawn;

    public float minBlankProbability;
    public float maxBlankProbability;

    [System.Serializable]
    public struct Obstacle {
        public float probability;
        public GameObject obstacleToSpawn;
    }

    public float initialSpawnDistance;
    public float spawnDistanceChangePerSpawn;
    public float minSpawnDistance;
    public float maxSpawnDistance;

    public List<Obstacle> obstacles;

    public GameObject GetObstacle() {
        float total = 0.0f;
        foreach(Obstacle obstacle in obstacles) {
            total += obstacle.probability;
        }

        float number = Random.Range(0, total);

        float currentTotal = 0;
        GameObject obstacleToReturn = null;
        foreach(Obstacle obstacle in obstacles) {
            if(currentTotal < number) {
                obstacleToReturn = obstacle.obstacleToSpawn;
            }
            currentTotal += obstacle.probability;
        }

        return obstacleToReturn;
    }
}
