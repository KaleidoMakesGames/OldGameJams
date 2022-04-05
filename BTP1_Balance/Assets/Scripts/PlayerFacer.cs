using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerFacer : MonoBehaviour {
    public GameObject player;
    public Vector2 offset;

    public float startScalingDistance;
    public float stopScalingDistance;
    public float maxScale;
    public float minScale;

	// Update is called once per frame
	void Update () {
		if(player == null) {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) {
                foreach (Collider2D collider in player.GetComponents<Collider2D>()) {
                    if (!collider.isTrigger) {
                        offset = new Vector3(collider.bounds.center.x, collider.bounds.min.y) - player.transform.position;
                    }
                }
            }
        }

        Vector3 playerPosition = player == null ? transform.position : player.transform.position + (Vector3)offset;
        transform.up = (playerPosition - transform.position).normalized;

        float distance = Vector2.Distance(playerPosition, transform.position);
        transform.localScale = new Vector3(transform.localScale.x, Mathf.Lerp(minScale, maxScale, Mathf.InverseLerp(stopScalingDistance, startScalingDistance, distance)), transform.localScale.z);

    }
}
