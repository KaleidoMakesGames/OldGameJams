using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoDisplayer : MonoBehaviour
{
    public GameManager manager;
    public TMPro.TextMeshProUGUI deathsField;
    public TMPro.TextMeshProUGUI rescuedField;

    public TMPro.TextMeshProUGUI titleField;
    public TMPro.TextMeshProUGUI reportField;
    public TMPro.TextMeshProUGUI zombiesSlainField;

    // Update is called once per frame
    void Update()
    {
        deathsField.text = manager.deathsSoFar.ToString();
        rescuedField.text = manager.rescued.ToString();

        titleField.text = "ALL CLEAR";
        reportField.text = string.Format("Your troop rescued <b><color=#00FF00>{0}</color></b> civilians and sustained <b><color=#FF0000>{1}</color></b> casualties.", manager.rescued, manager.deathsSoFar);
        zombiesSlainField.text = string.Format("<b><color=#FFFF00>{0}</color></b> zombies were harmed in the making of this video game.", manager.zombiesSlain);
    }
}
