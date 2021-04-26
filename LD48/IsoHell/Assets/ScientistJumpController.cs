using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientistJumpController : MonoBehaviour
{
    public float probabilityPerSecond;
    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        if(Random.Range(0.0f, 1.0f) < (probabilityPerSecond * Time.deltaTime)) {
            animator.SetTrigger("DoJump");
        }
    }
}
