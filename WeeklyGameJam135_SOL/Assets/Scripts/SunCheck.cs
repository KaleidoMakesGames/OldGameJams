using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SunCheck : MonoBehaviour
{
    public UILogic logic;

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.GetComponent<CometController>() != null) {
            CometController c = collision.GetComponent<CometController>();
            c.speedProgress = 0;
            c.currentState = CometController.State.Idle;

            float startPosHeight = Vector2.Distance(c.startPos.position, transform.position);
            Vector2 newAngle = (c.transform.position - transform.position).normalized;
            c.startPos.position = newAngle * startPosHeight;
            c.startPos.up = newAngle.normalized;
            c.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            logic.currentState = UILogic.State.IdleOnSun;
        }
    }
}
