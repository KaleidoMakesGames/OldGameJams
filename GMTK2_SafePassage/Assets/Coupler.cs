using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coupler : MonoBehaviour
{
    public GameObject a;

    // Update is called once per frame
    void Update()
    {
        if(a == null) {
            Destroy(gameObject);
        }
    }
}
