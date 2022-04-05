using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class audiomixercontroller : MonoBehaviour {
    public AudioSource silencer { get; private set; }

    public void ShouldSilentChannelBeOn(bool on) {
        if(silencer != null) {
            silencer.mute = !on;
        }
    }

    private void Update() {
        if(silencer == null) {
            AudioSourceSilencer silencerPossible = FindObjectOfType<AudioSourceSilencer>();
            silencer = silencerPossible == null ? null : silencerPossible.GetComponent<AudioSource>();
        }
    }
}
