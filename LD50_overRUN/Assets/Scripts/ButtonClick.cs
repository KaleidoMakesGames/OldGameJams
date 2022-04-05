using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    public AudioClip clip;
    private void Awake() {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate {
            GetComponent<AudioSource>().PlayOneShot(clip);
        });
    }

    public void Play() {
        GetComponent<AudioSource>().PlayOneShot(clip);
    }
}
