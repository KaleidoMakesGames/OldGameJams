using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomPitcher : MonoBehaviour {
    public float pitchMin;
    public float pitchMax;

    public void PlayAudio() {
        AudioSource source = GetComponent<AudioSource>();
        source.pitch = Random.Range(pitchMin, pitchMax);
        source.Play();
    }
}
