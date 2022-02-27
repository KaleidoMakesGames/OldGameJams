using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Collider2D attackCollider;
    public Transform cursor;
    public float attackHitForce;
    public float cooldownTime;
    private float _lastAttackTime;

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseVector = cursor.position - transform.position;
        attackCollider.transform.up = mouseVector;
        if(Input.GetButtonDown("Fire1")) {
            if (Time.time - _lastAttackTime >= cooldownTime) {
                DoAttack();
                _lastAttackTime = Time.time;
            }
        }
    }

    void DoAttack() {
        List<Collider2D> hits = new List<Collider2D>();
        attackCollider.OverlapCollider(new ContactFilter2D(), hits);
        foreach(var hit in hits) {
            if(!hit.isTrigger && hit.GetComponentInParent<AttackController>() != this) {
                Vector2 direction = hit.transform.position - transform.position;
                hit.GetComponentInParent<Rigidbody2D>().AddForce(direction.normalized * attackHitForce, ForceMode2D.Impulse);
            }
        }
    }
}
