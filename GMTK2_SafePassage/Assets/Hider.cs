using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hider : MonoBehaviour
{
    public float radius;

    public UnityEvent OnDone;

    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            if (Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position) <= radius) {
                OnDone.Invoke();
                gameObject.SetActive(false);
            }
        }
    }
}
