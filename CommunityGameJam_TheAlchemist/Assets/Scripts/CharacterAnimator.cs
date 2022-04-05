using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour {
    public Animator characterAnimator;
    public float groundOffset;

    // Update is called once per frame
    void LateUpdate()
    {
        characterAnimator.transform.up = Vector3.up;
        characterAnimator.transform.localPosition = Vector3.zero;
        characterAnimator.transform.Translate(Vector3.up * groundOffset, Space.Self);
        characterAnimator.SetFloat("facingX", transform.up.x);
        characterAnimator.SetFloat("facingY", transform.up.y);
    }
}
