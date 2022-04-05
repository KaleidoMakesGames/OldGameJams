using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class CarryEnabler : MonoBehaviour {
    public Collider2D objectCollider { get; private set; }
    public Animator animator;
    public float throwSpeed;
    public GameObject spawnOnDestroy;

    public UnityEvent OnThrow;
    public UnityEvent OnCrash;

    private bool isThrown;

    private bool _canPickUp;
    public bool canPickUp {
        get {
            return _canPickUp && !isThrown;
        }
        set {
            _canPickUp = value;
        }
    }

    [HideInInspector] public bool isCarried;

    private void Awake() {
        isThrown = false;
        canPickUp = true;
        objectCollider = GetComponent<Collider2D>();
    }

    private void Update() {
        objectCollider.enabled = !isCarried;
        if(isCarried) {
            objectCollider.isTrigger = true;
        }
        animator.SetBool("IsFloating", !isCarried && !isThrown);
    }

    public void Throw(Vector3 direction) {
        transform.parent = null;
        isThrown = true;
        OnThrow.Invoke();
        StartCoroutine(DoThrow(direction));
    }

    private IEnumerator DoThrow(Vector3 direction) {
        while (true) {
            transform.Translate(throwSpeed * direction * Time.deltaTime, Space.World);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (isThrown && !collision.isTrigger && collision.GetComponentInParent<Carrier>() == null) {
            if (spawnOnDestroy != null) {
                GameObject newObject = Instantiate(spawnOnDestroy);
                newObject.transform.position = transform.position;

                if (GetComponent<FlaskContents>().contents != null) {
                    Color c = GetComponent<FlaskContents>().contents.potionColor;
                    var m = newObject.GetComponent<ParticleSystem>().main;
                    var s = m.startColor;
                    s.colorMax = new Color(c.r, c.g, c.b, s.colorMax.a);
                    s.colorMin = new Color(c.r, c.g, c.b, s.colorMin.a);
                    m.startColor = s;
                }
                OnCrash.Invoke();
                Destroy(gameObject);
            }
        }
    }
}
