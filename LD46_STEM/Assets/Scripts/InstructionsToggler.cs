using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsToggler : MonoBehaviour
{
    public bool visible;

    public Animator animator;

    public void Toggle() {
        visible = !visible;
    }

    private void Update() {
        animator.SetBool("IsVisible", visible);
    }
}
