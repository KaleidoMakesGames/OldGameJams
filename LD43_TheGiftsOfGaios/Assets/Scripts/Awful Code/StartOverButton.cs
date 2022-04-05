using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartOverButton : MonoBehaviour {
    public void StartOver() {
        PlayerChoices.Instance.ResetChoices();
        SceneManager.LoadScene("Main Menu");
    }
}
