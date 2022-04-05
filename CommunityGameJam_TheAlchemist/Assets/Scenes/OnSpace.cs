using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnSpace : MonoBehaviour
{
    public UnityEvent OnSpacePressed;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space)) {
            OnSpacePressed.Invoke();
        }    
    }
}
