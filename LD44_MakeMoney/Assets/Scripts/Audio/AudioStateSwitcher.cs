using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioStateSwitcher : MonoBehaviour
{
    private AudioController controller;
    public void GoToState(int stateNumber) {
        if(controller == null) {
            controller = FindObjectOfType<AudioController>();
        }

        if(controller != null) {
            controller.currentPhase = (AudioController.GamePhase)stateNumber;
        }
    }
}
