using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackController : MonoBehaviour
{
    public MissileController missileControllerPrefab;
    public float fireRate;
    public List<Transform> firePositions;
    public UnityEvent OnFire;

    [System.Serializable]
    public enum FireMode { Random, Alternate, Simultaneous}
    public FireMode fireMode;

    private float _lastFireTime;
    private int nextToFire;
    public Vector3 fireTarget;
    
    public void TryFire() {
        if(!enabled) {
            return;
        }
        if(Time.time - _lastFireTime >= 1/fireRate) {
            _lastFireTime = Time.time;
            OnFire.Invoke();
            if (fireMode == FireMode.Alternate) {
                Fire(firePositions[nextToFire].position);
                nextToFire = (int)Mathf.Repeat(nextToFire + 1, firePositions.Count);
            } else if (fireMode == FireMode.Random) {
                Fire(firePositions[Random.Range(0, firePositions.Count)].position);
            } else {
                foreach (var firePosition in firePositions) {
                    Fire(firePosition.position);
                }
            }
        }
    }

    private void Fire(Vector3 source) {
        var mc = Instantiate(missileControllerPrefab.gameObject).GetComponent<MissileController>();
        mc.sourceObject = gameObject;
        mc.transform.position = source;
        mc.direction = fireTarget - source;
        mc.direction.y = 0.0f;
    }
}
