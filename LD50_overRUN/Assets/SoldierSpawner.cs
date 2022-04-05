using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SoldierSpawner : MonoBehaviour
{
    public BoxCollider2D spawnArea;
    public BoxCollider2D destinationArea;
    public SoldierController soldierPrefab;

    public int numSoldiersToMaintain;

    private void Update() {
        if (SoldierController.soldiers.Where(x => !x.isCivilian).Count() < numSoldiersToMaintain) {
            Spawn();
        }
    }

    public void Spawn() {
        var spawnPoint = spawnArea.transform.TransformPoint(new Vector2(Random.value, Random.value) * spawnArea.size - spawnArea.size / 2 + spawnArea.offset);
        var destinationPoint = destinationArea.transform.TransformPoint(new Vector2(Random.value, Random.value) * destinationArea.size - destinationArea.size / 2 + destinationArea.offset);

        var newSoldier = Instantiate(soldierPrefab.gameObject).GetComponent<SoldierController>();
        newSoldier.transform.position = spawnPoint;
        newSoldier.MoveTo(destinationPoint);
    }
}
