using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSoundMaker : MonoBehaviour
{
    public float chancePerSecond;
    public AudioClip c;
    public float minPitch;
    public float maxPitch;
    public float minVolume;
    public float maxVolume;
    
    // Update is called once per frame
    void Update()
    {
        float secondsSince = Time.deltaTime;
        float probability = chancePerSecond * secondsSince;
        if(Random.Range(0.0f, 1.0f) < probability) {
            DoPlay();
        }
    }

    void DoPlay() {
        AudioSource newS = new GameObject("ExplosionSound", typeof(AudioSource)).GetComponent<AudioSource>();
        newS.transform.SetParent(transform);
        newS.pitch = Random.Range(minPitch, maxPitch);
        newS.panStereo = Random.Range(-1.0f, 1.0f);
        newS.volume = Random.Range(minVolume, maxVolume);
        newS.PlayOneShot(c);

        StartCoroutine(MonitorSound(newS));
    }

    IEnumerator MonitorSound(AudioSource newS) {
        while(true) {
            if(!newS.isPlaying) {
                Destroy(newS.gameObject);
                yield break;
            }
            yield return null;
        }
    }
}
