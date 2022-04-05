using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;
    public void DoPlay() {
        audioSource.PlayOneShot(audioClip);
    }
}
