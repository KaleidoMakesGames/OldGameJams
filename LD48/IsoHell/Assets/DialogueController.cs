using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class DialogueController : MonoBehaviour
{
    public TextMeshProUGUI textField;
    public GameObject nextIndicator;

    public UnityEvent OnSpeak;
    public UnityEvent OnStartSpeak;
    public UnityEvent OnStopSpeak;

    [HideInInspector] public UnityEvent OnDialogueFinished;

    private Dialogue currentDialogue;
    private int currentItem;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            if(currentDialogue != null) {
                ShowNext();
            }
        }
    }

    public void EnqueueDialogue(Dialogue dialogue) {
        currentDialogue = dialogue;
        currentItem = -1;
        OnStartSpeak.Invoke();
        ShowNext();
    }

    private void ShowNext() {
        currentItem++;
        if(currentItem < 0 || currentItem >= currentDialogue.dialogue.Count) {
            nextIndicator.SetActive(false);
            currentDialogue = null;
            textField.text = "";
            OnStopSpeak.Invoke();
            OnDialogueFinished.Invoke();
            OnDialogueFinished.RemoveAllListeners();
            currentItem = -1;
        } else {
            nextIndicator.SetActive(true);
            textField.text = currentDialogue.dialogue[currentItem];
        }
    }
}
