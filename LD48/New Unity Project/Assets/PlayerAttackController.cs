using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    public MissileController missileController;
    public float fireRate;

    private float _lastFireTime;

    // Update is called once per frame
    void Update()
    {
        var worldRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane originPlane = new Plane(Vector3.up, transform.position);
        float enter;
        if (originPlane.Raycast(worldRay, out enter)) {
            Vector3 position = worldRay.origin + worldRay.direction * enter;
            transform.LookAt(position, Vector3.up);
        }

        if (Input.GetMouseButton(0)) {
            TryFire();
        }
    }

    private void TryFire() {
        if(Time.time - _lastFireTime >= 1/fireRate) {
            
            var mc = Instantiate(missileController.gameObject).GetComponent<MissileController>();
            mc.sourceObject = gameObject;
            mc.transform.position = transform.position;
            mc.direction = transform.forward;
            _lastFireTime = Time.time;
        }
    }
}
