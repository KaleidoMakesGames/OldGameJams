using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Onboarding : MonoBehaviour
{
    public TextMeshProUGUI textField;
    public Carrier carrier;
    public Leogan leogan;
    public Cauldron cauldron;
    public MovementController movement;

    private void Awake() {
        textField.text = "Use the W, S, A, and D keys to move.";
        movement.OnMove.AddListener(OnFirstMove);
    }

    private void OnFirstMove() {
        textField.text = "Walk over to the floating flask and press SPACE to pick it up.";
        movement.OnMove.RemoveListener(OnFirstMove);
        carrier.OnPickup.AddListener(OnFirstPickup);
    }

    private void OnFirstPickup() {
        textField.text = "Bring the flask to one of the four barrels of essence and press SPACE to fill it up.";
        carrier.OnPickup.RemoveListener(OnFirstPickup);
        carrier.OnFill.AddListener(OnFirstFill);
    }

    private void OnFirstFill() {
        textField.text = "Walk over to the magic cauldron and press SPACE to dump the flask in.";
        carrier.OnFill.RemoveListener(OnFirstFill);
        carrier.OnPour.AddListener(OnFirstPour);
    }

    private void OnFirstPour() {
        textField.text = "Great. Add another essence to make a potion.";
        carrier.OnPour.RemoveListener(OnFirstPour);
        cauldron.OnPotionMade.AddListener(OnFirstSucceed);
    }

    private void OnFirstSucceed() {
        textField.text = "Great job! Now pick up that potion, walk over to a counter, and press SPACE to set it down for a moment.";
        cauldron.OnPotionMade.RemoveListener(OnFirstSucceed);
        carrier.OnSetDown.AddListener(OnSetDown);
    }

    private void OnSetDown() {
        leogan.gameObject.SetActive(true);
        gameObject.SetActive(false);
        carrier.OnSetDown.RemoveListener(OnSetDown);
    }
}
