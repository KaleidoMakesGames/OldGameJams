using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ConvoController : MonoBehaviour
{
    public UnityEvent OnConvoFinished;

    public GameObject textUI;
    public TextMeshProUGUI textField;

    public bool isTalking;
    public bool isAlert;

    public Dialogue currentDialogue;

    private int currentLine;

    public void StartConvo() {
        isTalking = true;
        isAlert = false;
        currentLine = 0;
    }

    public void SetDialogue(Dialogue d) {
        currentDialogue = d;
    }

    private void Update() {
        if (isTalking) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                currentLine++;
            }

            if (currentLine > currentDialogue.dialogue.Count - 1) {
                isTalking = false;
                OnConvoFinished.Invoke();
            } else {
                textField.text = currentDialogue.dialogue[currentLine];
            }
        }

        if(isAlert) {
            textField.text = "!";
        }

        textUI.SetActive(isTalking || isAlert);
    }
}
