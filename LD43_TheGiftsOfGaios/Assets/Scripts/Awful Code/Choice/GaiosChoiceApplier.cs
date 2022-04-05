using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaiosChoiceApplier : MonoBehaviour {
    private CharacterAttackController cac;
    private CharacterMovementController cmc;

    private void Start() {
        cac = GetComponent<CharacterAttackController>();
        cmc = GetComponent<CharacterMovementController>();
    }

    private void Update() {
        cmc.canDash = PlayerChoices.Instance.canDash;
        cmc.infinityJump = PlayerChoices.Instance.canInfiniJump;
        cac.canAttack = PlayerChoices.Instance.canAttack;
    }
}
