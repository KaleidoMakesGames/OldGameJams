using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DialogDisplayer : MonoBehaviour
{
    private TextMeshProUGUI text;

    public DialogSet set;
    public GameObject disableObject;

    public bool loop;

    public float charactersPerSecond;

    public UnityEvent OnFinishedDialogue;

    private int currentIndex;

    private float currentTextShownIndex;

    private void Awake() {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void Restart() {
        currentTextShownIndex = 0;
        currentIndex = 0;
    }

    public void SetShown(bool show) {
        disableObject.SetActive(show);
    }

    private void Update() {
        if(currentIndex < set.lines.Count) {
            string line = set.lines[currentIndex].text;
            if (line != "") {
                currentTextShownIndex = Mathf.Clamp(currentTextShownIndex + (charactersPerSecond * Time.deltaTime), 1, line.Length);

                string subString = line.Substring(0, Mathf.FloorToInt(currentTextShownIndex));
                text.text = subString;
            }
        }
        if(Input.GetKeyUp(KeyCode.Space)) {
            Advance();
        }
    }

    public void Advance() {
        if(currentIndex == set.lines.Count-1) {
            OnFinishedDialogue.Invoke();
        } else {
            currentTextShownIndex = 0;
            currentIndex++;
        }
    }
}
