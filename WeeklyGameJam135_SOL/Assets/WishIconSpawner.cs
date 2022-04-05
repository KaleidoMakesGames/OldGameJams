using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WishIconSpawner : MonoBehaviour
{
    public GameObject toSpawn;

    public WishTracker tracker;

    public Transform container;


    private int lastNumber;

    // Start is called before the first frame update
    void Start()
    {
        lastNumber = tracker.currentNumberOfWishes;
        Respawn();
    }

    // Update is called once per frame
    void Update()
    {
        if(lastNumber != tracker.currentNumberOfWishes) {
            Respawn();
        }
        lastNumber = tracker.currentNumberOfWishes;
    }

    void Respawn() {
        foreach(Transform t in container) {
            Destroy(t.gameObject);
        }

        for(int i = 0; i < tracker.currentNumberOfWishes; i++) {
            GameObject o = Instantiate(toSpawn, container);
        }
    }
}
