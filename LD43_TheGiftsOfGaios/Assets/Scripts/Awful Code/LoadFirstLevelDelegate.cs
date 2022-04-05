using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadFirstLevelDelegate : MonoBehaviour {
    public void LoadFirstLevel() {
        PlayerChoices.Instance.FinishedChoosing();
    }
}
