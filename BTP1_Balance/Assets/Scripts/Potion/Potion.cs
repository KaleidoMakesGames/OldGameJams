using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class Potion : MonoBehaviour {
    public PotionSettings potionSettings;

    private void Update() {
        transform.localScale = Vector3.one * potionSettings.size;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
#if UNITY_EDITOR
        if(!EditorApplication.isPlaying) {
            return;
        }
#endif
        CharacterTeamAssigner teamAssigner = collision.GetComponent<CharacterTeamAssigner>();
        if(teamAssigner != null && teamAssigner.team == potionSettings.teamToHeal) {
            HealthTracker tracker = collision.GetComponent<HealthTracker>();
            tracker.RestoreHealth(potionSettings.healAmount);
        }
        gameObject.SetActive(false);
    }
}
