using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOFolder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for(int i = transform.childCount-1; i >= 0; i--) { 
            transform.GetChild(i).SetParent(null, true);
        }   
    }
}
