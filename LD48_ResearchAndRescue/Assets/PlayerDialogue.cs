using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerDialogue : MonoBehaviour
{
    public TextMeshProUGUI textField;
    public GameObject enableObject;
    public ElevatorController ec;
    
    public float initialTime;

    private bool initialDone;

    private void Start() {
        StartCoroutine(InitialText());
    }

    private IEnumerator InitialText() {
        textField.text = "Don't worry, I'm coming to help!";
        yield return new WaitForSeconds(initialTime);
        textField.text = "";
        initialDone = true;
    }

    private void Update() {
        transform.rotation = Camera.main.transform.rotation;
        enableObject.SetActive(textField.text != "");
        if (!initialDone) {
            return;
        }
        if(ec.isInElevator) {
            if(ec.canUseElevator) {
                textField.text = "Press E to use elevator.";
            } else {
                textField.text = "I must rescue the researchers!";
            }
        } else {
            textField.text = "";
        }
    }

}
