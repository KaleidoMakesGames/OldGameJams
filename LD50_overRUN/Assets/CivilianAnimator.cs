using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianAnimator : MonoBehaviour
{
    public float jumpsPerSecond;
    public Animator animator;

    private SoldierController s;
    private void Awake() {
        s = GetComponent<SoldierController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!s.agent.isMoving && StatsUtils.SamplePoisson(jumpsPerSecond * Time.deltaTime) > 0) {
            animator.SetTrigger("DoJump");
        }
    }
}
