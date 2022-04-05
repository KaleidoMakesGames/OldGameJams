using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public KCharacterController playerCharacter;
    public WeaponController playerWeapon;
    
    // Update is called once per frame
    void Update()
    {
        Vector2Int drive = Vector2Int.zero;
        drive += Input.GetKey(KeyCode.W) ? Vector2Int.up : Vector2Int.zero;
        drive += Input.GetKey(KeyCode.S) ? Vector2Int.down : Vector2Int.zero;
        drive += Input.GetKey(KeyCode.A) ? Vector2Int.left : Vector2Int.zero;
        drive += Input.GetKey(KeyCode.D) ? Vector2Int.right : Vector2Int.zero;
        playerCharacter.drive = drive;

        playerWeapon.triggerPressed = Input.GetMouseButton(0);
        playerWeapon.fireDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - playerWeapon.transform.TransformPoint(playerWeapon.firePoint);
    }
}
