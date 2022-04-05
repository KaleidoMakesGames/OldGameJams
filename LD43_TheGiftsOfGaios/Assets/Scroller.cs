using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Scroller : MonoBehaviour {
    public UnityEvent OnScroll;
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetMouseButtonUp(0)) {
            OnScroll.Invoke();
        }	
	}
}
