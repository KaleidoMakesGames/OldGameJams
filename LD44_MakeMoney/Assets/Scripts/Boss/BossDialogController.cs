using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossDialogController : MonoBehaviour
{
    public TextMeshProUGUI textField;
    public Transform showHider;

    public BossDialog currentDialog;

    private float timeSet;

    private void Start() {
        ShowDialog(currentDialog);
    }

    public void ShowDialog(BossDialog dialog) {
        currentDialog = dialog;
        timeSet = Time.time;
    }

    private void Update() {
        if(currentDialog != null) {
            showHider.gameObject.SetActive(true);
            textField.text = currentDialog.text;
            if(Time.time-timeSet >= currentDialog.timeToShow) {
                ShowDialog(null);
            }
        } else {
            showHider.gameObject.SetActive(false);
        }
    }
}
