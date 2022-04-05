using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public Vector2 zoneSize;
    public float expectedZombiesPerSecond;
    public ZombieController zombiePrefab;

    // Update is called once per frame
    void Update()
    {
        float expectedZombiesInTimePeriod = expectedZombiesPerSecond * Time.deltaTime;
        int numToSpawn = StatsUtils.SamplePoisson(expectedZombiesInTimePeriod);
        for(int i = 0; i < numToSpawn; i++) {
            DoSpawn();
        }
    }

    void DoSpawn() {
        if(ZombieController.zombiesList != null && ZombieController.zombiesList.Count >= GameManager.instance.maxZombies) {
            return;
        }
        float x = Random.value * zoneSize.x;
        float y = Random.value * zoneSize.y;
        var spawnPos = new Vector2(x, y) - zoneSize / 2;
        Instantiate(zombiePrefab.gameObject).transform.position = transform.TransformPoint(spawnPos);
    }

    private void OnDrawGizmos() {
        var bounds = new Bounds(Vector2.zero, zoneSize);
        var a = transform.TransformPoint(new Vector2(bounds.min.x, bounds.min.y));
        var b = transform.TransformPoint(new Vector2(bounds.max.x, bounds.min.y));
        var c = transform.TransformPoint(new Vector2(bounds.max.x, bounds.max.y));
        var d = transform.TransformPoint(new Vector2(bounds.min.x, bounds.max.y));
        Gizmos.DrawLine(a, b);
        Gizmos.DrawLine(b, c);
        Gizmos.DrawLine(c, d);
        Gizmos.DrawLine(d, a);
    }
}
