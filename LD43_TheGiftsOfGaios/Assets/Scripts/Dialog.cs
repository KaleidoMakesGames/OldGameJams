using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Dialog : MonoBehaviour {
    [TextArea] public List<string> dialogBlocks;

    TextMeshProUGUI text;

    private int currentNumber = 0;

    public UnityEvent OnDialogDone;

    private void Start() {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update() {
        if(currentNumber < dialogBlocks.Count) {
            text.text = dialogBlocks[currentNumber];
        }
    }

    public void Advance() {
        currentNumber++;
        if(currentNumber >= dialogBlocks.Count) {
            OnDialogDone.Invoke();
        }
    }
}
