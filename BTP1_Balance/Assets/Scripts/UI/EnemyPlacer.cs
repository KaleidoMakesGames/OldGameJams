using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EnemyPlacer : MonoBehaviour {
    public GameObject enemyPrefab;
    public PlaceableObjectPlacer placer;

    public UnityEvent OnPlaced;
    public UnityEvent OnRemoved;

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonUp(0)) {
            if (EventSystem.current.IsPointerOverGameObject()) {
                return;
            }

            Vector2 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            bool removed = false;
            foreach(RaycastHit2D hit in Physics2D.RaycastAll(point, Vector2.zero)) {
                if(hit.collider.GetComponent<IPlaceableObject>() != null) {
                    placer.RemoveObject(hit.collider.gameObject);
                    removed = true;
                    OnRemoved.Invoke();
                }
            }

            if (!removed) {
                Collider2D hitCollider = Physics2D.Raycast(point, Vector2.zero, 0.0f, LayerMask.GetMask("Spawnable")).collider;
                if (hitCollider != null) {
                    Level level = hitCollider.GetComponentInParent<Level>();
                    if (level == null) {
                        Debug.LogError("Spawnable collider should have a Level as its parent!");
                    } else {
                        placer.PlaceObject(enemyPrefab, point, level.transform);
                        OnPlaced.Invoke();
                    }
                }
            }
        }
    }
}
