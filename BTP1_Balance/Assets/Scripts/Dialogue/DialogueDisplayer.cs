using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DialogueDisplayer : MonoBehaviour {
    public class DialogueEvent : UnityEvent<int> { }

    public Dialogue dialogue;

    public bool startOnStart;

    private int currentDialogueNumber;
    public UnityEvent OnDialogueFinished;

    private TextMeshProUGUI text;

    private IEnumerator dialogueProcess;

    private void Awake() {
        currentDialogueNumber = 0;
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Start() {
        if(startOnStart) {
            StartDialogue();
        }
    }

    public void StartDialogue() {
        if(dialogueProcess != null) {
            StopCoroutine(dialogueProcess);
        }

        currentDialogueNumber = 0;
        dialogueProcess = RunDialogue();
        StartCoroutine(dialogueProcess);
    }

    private IEnumerator RunDialogue() {
        while(currentDialogueNumber < dialogue.dialogue.Count) {
            text.text = dialogue.dialogue[currentDialogueNumber].text;
            yield return new WaitForSeconds(dialogue.dialogue[currentDialogueNumber].secondsToDisplay);
            currentDialogueNumber++;
        }
        OnDialogueFinished.Invoke();
    }
}
