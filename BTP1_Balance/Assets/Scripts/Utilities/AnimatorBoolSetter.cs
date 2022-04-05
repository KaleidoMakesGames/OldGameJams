using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorBoolSetter : MonoBehaviour {
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void BoolOn(string name) {
        animator.SetBool(name, true);
    }

    public void BoolOff(string name) {
        animator.SetBool(name, false);
    }
}
