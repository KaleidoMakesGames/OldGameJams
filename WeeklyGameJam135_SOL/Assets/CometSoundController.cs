using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometSoundController : MonoBehaviour
{
    public AudioClip whooshSound;
    public AudioSource source;

    public CometController cc;

    public float freqYintercept;
    public float freqSlope;

    private void Start() {
        cc.orbiter.onLoop.AddListener(delegate () {
            if(Input.GetKey(KeyCode.Space)) {
                source.PlayOneShot(whooshSound);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        if(cc.currentState == CometController.State.InOrbit) {
            source.pitch = freqYintercept + freqSlope * Mathf.Abs(cc.orbiter.orbitSpeed / cc.orbiter.orbitRadius);
            if (Input.GetKeyDown(KeyCode.Space)) {
                source.PlayOneShot(whooshSound);
            }
        }    
    }
}
