using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Cauldron : MonoBehaviour {
    public FlaskContents newPotionPrefab;

    public PlacementPad newSpot;

    public RecipeBook book;

    public List<Potion> contents;

    public GameObject uiObject;
    public TextMeshProUGUI textField;

    public float spitSpeed;

    public bool acceptingContents { get; private set; }
    private bool showingText;

    public UnityEvent OnPotionMade;


    private void Awake() {
        acceptingContents = true;
        textField.text = "What shall we brew today?";
    }

    public void TakePotion(FlaskContents c) {
        contents.Add(c.contents);
        CheckContents();
    }

    private void Update() {
        uiObject.gameObject.SetActive(textField.text != "");

        if (contents.Count > 0 && !showingText) {
            string text = "";
            for (int i = 0; i < contents.Count; i++) {
                Potion p = contents[i];
                text += p.FString();

                if (contents.Count > 1) {
                    text += ", ";
                } else {
                    text += " ";
                }

                if (i == contents.Count - 1) {
                    text += "and...";
                }
            }
            textField.text = text;
        }
    }

    public void CheckContents() {
        // Try to find any recipe that includes these contents
        bool partialFound = false;
        foreach (Potion r in book.potions) {
            switch (r.Compare(contents)) {
                case Potion.MatchType.Full:
                    contents.Clear();
                    if (!newSpot.IsEmpty(null)) {
                        StartCoroutine(WaitForSpotClear(r));
                    } else {
                        CreateFromRecipe(r);
                    }
                    return;
                case Potion.MatchType.Partial:
                    partialFound = true;
                    break;
                case Potion.MatchType.None:
                    break;
            }
        }

        if (!partialFound) {
            SpitOut();
        }
    }

    private void SpitOut() {
        contents.Clear();
        ShowMessage("Yuck! What am I supposed to make with THAT?", 4.0f, delegate () {
            textField.text = "Let's try something else.";
        });
    }

    private delegate void Callback();

    private Coroutine textShowCoroutine;
    private void ShowMessage(string message, float time, Callback c = null) {
        if (textShowCoroutine != null) {
            StopCoroutine(textShowCoroutine);
        }
        textShowCoroutine = StartCoroutine(ShowMessageCoroutine(message, time, c));
    }

    private IEnumerator ShowMessageCoroutine(string message, float time, Callback c = null) {
        textField.text = message;
        showingText = true;
        yield return new WaitForSeconds(time);
        showingText = false;
        if (c != null) {
            c();
        }
    }

    private IEnumerator WaitForSpotClear(Potion r) {
        acceptingContents = false;

        showingText = true;
        textField.text = "Get that flask out of my way! I've got a potion ready!";

        while (!newSpot.IsEmpty(null)) {
            yield return null;
        }

        acceptingContents = true;
        showingText = false;
        CreateFromRecipe(r);
    }

    private void CreateFromRecipe(Potion r) {
        FlaskContents newFlask = Instantiate(newPotionPrefab.gameObject).GetComponent<FlaskContents>();
        newFlask.contents = r;
        newFlask.transform.parent = transform;
        newFlask.transform.position = transform.position;

        StartCoroutine(SpitToPoint(newFlask));
        ShowMessage("WOO-HOO! We've made " + r.FString() + "!", 4.0f, delegate() {
            textField.text = "What shall we brew next?";
        });

        OnPotionMade.Invoke();
    }

    private IEnumerator SpitToPoint(FlaskContents flaskContents) {
        flaskContents.GetComponent<CarryEnabler>().canPickUp = false;
        while (!Mathf.Approximately(Vector3.Distance(flaskContents.transform.position, newSpot.transform.position), 0.0f)) {
            flaskContents.transform.position = Vector3.MoveTowards(flaskContents.transform.position, newSpot.transform.position, spitSpeed * Time.deltaTime);
            yield return null;
        }
        flaskContents.transform.parent = null;
        flaskContents.GetComponent<CarryEnabler>().canPickUp = true;
    }
}
