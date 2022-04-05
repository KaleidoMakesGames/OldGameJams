using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerOption : MonoBehaviour
{
    public Spawnable prefab;
    public TMPro.TextMeshProUGUI textField;

    public UnityEngine.UI.Image i;

    public Color selectedColor = Color.green;
    public Color unselectedColor = Color.gray;

    public KeyCode code;

    public Spawnable hologram { get; private set; }

    private void Awake() {
        hologram = Instantiate(prefab.gameObject).GetComponent<Spawnable>();
        foreach(Collider2D c in hologram.GetComponentsInChildren<Collider2D>()) {
            Destroy(c);
        }
        foreach (Rigidbody2D c in hologram.GetComponentsInChildren<Rigidbody2D>()) {
            Destroy(c);
        }
        hologram.gameObject.SetActive(false);

        i.color = unselectedColor;

        Spawner s = FindObjectOfType<Spawner>();
        if(s.toSpawn == null) {
            DoSelect();
        }
    }

    public void SetHologramColor(Color c) {
        foreach(SpriteRenderer r in hologram.GetComponentsInChildren<SpriteRenderer>()) {
            r.color = c;
        }
    }

    // Update is called once per frame
    void Update()
    {
        textField.text = "(" + code.ToString() + ")\n" + prefab.name;
        if(Input.GetKeyDown(code)) {
            DoSelect();
        }
    }

    public void DoSelect() {
        Spawner s = FindObjectOfType<Spawner>();
        s.Select(this);
    }
}
