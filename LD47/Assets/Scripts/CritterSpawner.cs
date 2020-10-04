using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

public class CritterSpawner : MonoBehaviour
{
    public CritterAIController prefab;

    public Collider2D boundaryCollider;

    public int initial;
    public float probabilityPerSecond;

    private void Start() {
        for(int i = 0; i < initial; i++) {
            Spawn();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Random.Range(0.0f, 1.0f) < probabilityPerSecond*Time.fixedDeltaTime) {
            Spawn();
        }
    }

    private Vector2 GetRandomPoint() {
        Bounds b = boundaryCollider.bounds;
        if (b.size == Vector3.zero) {
            return b.center;
        }

        Vector2 p = new Vector2(Random.Range(b.min.x, b.max.x),
            Random.Range(b.min.y, b.max.y));
        while(!boundaryCollider.OverlapPoint(p)) {
            p = new Vector2(Random.Range(b.min.x, b.max.x),
            Random.Range(b.min.y, b.max.y));
        }
        return p;
    }

    public void Spawn() {
        Instantiate(prefab.gameObject).transform.position = GetRandomPoint();
    }
}
