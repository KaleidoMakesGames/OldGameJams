using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueSystem : MonoBehaviour
{
    public TMPro.TextMeshProUGUI textField;
    public TMPro.TextMeshProUGUI buttonField;
    public UnityEngine.UI.Button advanceButton;
    public GameObject dialogueBox;

    public UnityEvent OnStart;
    public UnityEvent OnShowGuides;
    public UnityEvent OnHideGuides;

    [System.Serializable]
    public struct DialogueItem {
        [TextArea] public string text;
        public string buttonText;
        public float time;

        public UnityEvent OnCompleteEvent;

        public DialogueItem(string text, string buttonText, float time, UnityEvent OnCompleteEvent = null) {
            this.text = text;
            this.buttonText = buttonText;
            this.time = time;
            this.OnCompleteEvent = OnCompleteEvent;
        }
    }

    public bool isDisplayingDialogue {
        get {
            return dialogueBox.activeSelf;
        }
    }

    private Queue<DialogueItem> queue;

    private Vector3 offset;

    private void Awake() {
        queue = new Queue<DialogueItem>();
        advanceButton.onClick.AddListener(Advance);

        offset = dialogueBox.transform.position - transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        Enqueue(new DialogueItem("Hello? Is anyone there?", "I'm here!", 0));
        Enqueue(new DialogueItem("Phew. Finally someone found me.", ">", 0));
        Enqueue(new DialogueItem("Looks like I'm in trouble here. I'm stuck on this mountain, \nand I don't think I can make it down on foot.", ">", 0));
        Enqueue(new DialogueItem("My truck is still working, but I ran over something \non my way up here and now my brakes are completely busted.", ">", 0));
        Enqueue(new DialogueItem("I can try to drive it down, \nbut there will definitely be some obstacles on the way.", ">", 0));
        Enqueue(new DialogueItem("Think you can help me get down safely?", "Sure. What can I do?", 0, OnShowGuides));
        Enqueue(new DialogueItem("Try building a plank to get me over that gap.", "", 0, OnHideGuides));
        Enqueue(new DialogueItem("Well, let's see if it worked... Ready?", "Go for it!", 0, OnStart));
        
        if(GetComponent<CarController>().initialCheckpoint != null) {
            while(queue.Count > 0) {
                var s = queue.Dequeue();
                if (s.OnCompleteEvent != null) {
                    s.OnCompleteEvent.Invoke();
                }
            }
            Advance();
        }
    }

    private void Update() {
        dialogueBox.transform.position = transform.position + offset;
    }

    public void Enqueue(DialogueItem i) {
        queue.Enqueue(i);
        if(queue.Count == 1) {
            Present();
        }
    }

    private IEnumerator AdvanceAfter(float time) {
        yield return new WaitForSeconds(time);
        Advance();
    }

    public void Advance() {
        if(queue.Count == 0) {
            dialogueBox.SetActive(false);
            return;
        }
        DialogueItem d = queue.Dequeue();
        if(d.OnCompleteEvent != null) {
            d.OnCompleteEvent.Invoke();
        }
        if (queue.Count == 0) {
            dialogueBox.SetActive(false);
            return;
        } else {
            Present();
        }
    }

    private void Present() {
        dialogueBox.SetActive(true);
        DialogueItem d = queue.Peek();
        textField.text = d.text;
        buttonField.text = d.buttonText;
        advanceButton.gameObject.SetActive(buttonField.text != "");
        if (d.time > 0) {
            StartCoroutine(AdvanceAfter(d.time));
        }
    }
}

/*
 * Looks like I'm in trouble here. The brakes are completely busted.

It's a long way down the mountain, and I don't think I can make it on foot.

I'm just going to put the pedal to the medal and see if I can make it.

Think you can help me get down safely?
*/