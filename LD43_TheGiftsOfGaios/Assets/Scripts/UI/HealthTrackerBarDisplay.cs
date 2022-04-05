using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTrackerBarDisplay : MonoBehaviour {
    public HealthTracker trackerToDisplay;
    
    public bool trackGaios;

    private RectTransform myRect;

	// Use this for initialization
	void Start () {
        myRect = transform.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        if(trackGaios && trackerToDisplay == null) {
            trackerToDisplay = PlayerChoices.FindGaiosComponent<HealthTracker>();
        }
		if(trackerToDisplay != null) {
            float percentage = trackerToDisplay.currentHealth/trackerToDisplay.maxHealth;
            myRect.anchorMax = new Vector2(percentage, myRect.anchorMax.y);
        }
	}

    private void OnValidate() {
        Start();
    }
}
