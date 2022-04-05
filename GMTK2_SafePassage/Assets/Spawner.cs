using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float deleteRadius;

    public Color hologramTint = Color.blue;
    public Color deleteTint = Color.red;

    public GameObject deleteTooltip;

    private static List<Spawnable> spawned;

    public SpawnerOption toSpawn { get; private set; }

    public GameObject canvas;

    private Spawnable deleteHover = null;

    private void Awake() {
        spawned = new List<Spawnable>();
    }

    public void Select(SpawnerOption s) {
        if(toSpawn != null) {
            toSpawn.hologram.gameObject.SetActive(false);
            toSpawn.i.color = toSpawn.unselectedColor;
        }
        toSpawn = s;
        toSpawn.hologram.transform.SetParent(transform);
        toSpawn.hologram.transform.localPosition = Vector2.zero;
        toSpawn.i.color = toSpawn.selectedColor;
        toSpawn.SetHologramColor(hologramTint);
    }

    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
            canvas.gameObject.SetActive(false);
            if(toSpawn != null) {
                toSpawn.hologram.gameObject.SetActive(false);
            }
            return;
        }
        canvas.gameObject.SetActive(true);
        transform.position = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (toSpawn != null) {
            toSpawn.hologram.gameObject.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0)) {
            GameObject newO = Instantiate(toSpawn.prefab.gameObject);
            newO.transform.position = transform.position;
            spawned.Add(newO.GetComponent<Spawnable>());
        }    

        if(!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
            Spawnable toDelete = null;
            float closestDistance = Mathf.Infinity;
            foreach(Collider2D d in Physics2D.OverlapCircleAll(transform.position, deleteRadius)) {
                Spawnable s = d.GetComponentInParent<Spawnable>();
                if(s != null) {
                    Vector2 closestOnCollider = d.ClosestPoint(transform.position);
                    float dist = Vector2.Distance(closestOnCollider, transform.position);
                    if(toDelete == null || dist < closestDistance) {
                        toDelete = s;
                    }
                }
            }

            if(toDelete != deleteHover) {
                if(deleteHover != null) {
                    deleteHover.Untint();
                }
                
                deleteHover = toDelete;
                if (deleteHover != null) {
                    deleteHover.Tint(deleteTint);
                }
            }

            if(Input.GetMouseButtonUp(1)) {
                if (deleteHover != null) {
                    Destroy(deleteHover.gameObject);
                    deleteHover = null;
                }
            }

            deleteTooltip.SetActive(deleteHover != null);
        }
    }

    public void Stop() {
        gameObject.SetActive(false);
        if(toSpawn != null) {
            toSpawn.hologram.gameObject.SetActive(false);
        }
    }
        

    public static void ClearSpawned() {
        foreach(Spawnable o in spawned) {
            if (o != null) {
                Destroy(o.gameObject);
            }
        }
        spawned.Clear();
    }
}
