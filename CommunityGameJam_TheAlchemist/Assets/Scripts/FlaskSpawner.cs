using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlaskSpawner : MonoBehaviour
{
    public CarryEnabler carryPrefab;
    public PlacementPad pad;

    public float delayUntilSpawn;
    public float spawnSpeed;

    public bool spawning;

    // Update is called once per frame
    void Update()
    {
        if(!spawning) {
            if(pad.IsEmpty(null)) {
                StartCoroutine(DoSpawn());
            }
        }
    }

    public IEnumerator DoSpawn() {
        spawning = true;
        yield return new WaitForSeconds(delayUntilSpawn);
        CarryEnabler newObject = Instantiate(carryPrefab.gameObject, transform).GetComponent<CarryEnabler>();
        newObject.transform.localEulerAngles = Vector3.zero;
        newObject.transform.localPosition = Vector3.zero;
        newObject.canPickUp = false;
        newObject.isCarried = false;
        while(!Mathf.Approximately(Vector3.Distance(newObject.transform.position, pad.transform.position), 0.0f)) {
            newObject.transform.position = Vector3.MoveTowards(newObject.transform.position, pad.transform.position, spawnSpeed * Time.deltaTime);
            yield return null;
        }
        newObject.transform.parent = null;
        newObject.canPickUp = true;
        spawning = false;
    }
}
