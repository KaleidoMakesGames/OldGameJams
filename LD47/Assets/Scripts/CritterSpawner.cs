using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

public class CritterSpawner : MonoBehaviour
{
    public BoxCollider2D goodPrefab;
    public BoxCollider2D badPrefab;

    public Vector2 size;

    public int numGood;
    public int numBad;
    public int cyclesToGiveUp;

    private void Start() {
        for(int i = 0; i < numGood; i++) {
            Spawn(goodPrefab);
        }
        for(int i = 0; i < numBad; i++) {
            Spawn(badPrefab);
        }
    }

    private Vector2 GetRandomPoint() {
        Vector2 p = new Vector2(Random.Range(-size.x/2, size.x/2),
            Random.Range(-size.y/2, size.y/2));
        return transform.TransformPoint(p);
    }

    public void Spawn(BoxCollider2D collider) {
        Bounds b = new Bounds(Vector2.zero, collider.size);
        for (int i = 0; i < cyclesToGiveUp; i++) {
            Vector2 randomPoint = GetRandomPoint();
            b.center = randomPoint;
            var atArea = Physics2D.OverlapArea(b.min, b.max);
            if (atArea == null) {
                Instantiate(collider.gameObject).transform.position = randomPoint;
                return;
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, transform.TransformVector(size));
    }
}
