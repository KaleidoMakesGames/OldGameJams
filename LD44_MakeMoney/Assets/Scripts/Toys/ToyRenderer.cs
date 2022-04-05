using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[ExecuteInEditMode]
public class ToyRenderer : MonoBehaviour {
    public ToyController controller;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // Update is called once per frame
    void Update() {
        if(spriteRenderer == null) {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if(controller == null) {
            controller = GetComponentInParent<ToyController>();
        }

        if (spriteRenderer != null && controller != null && controller.toyType != null) {
            spriteRenderer.color = controller.toyType.color;
            spriteRenderer.sprite = controller.toyType.sprite;
        }

        if(animator == null) {
            animator = GetComponent<Animator>();
        }

        animator.SetBool("IsPickedUp", controller.isPickedUp);
        animator.SetBool("CanPutDown", controller.canPutDown);
    }

    private void Reset() {
        controller = GetComponentInParent<ToyController>();
    }
}
