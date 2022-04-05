using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Carrier : MonoBehaviour
{
    public Collider2D pickupCollider;
    public CarryEnabler heldObject;
    public Transform holdSpot;
    public float holdOffset;

    public Potion antidotePotion;
    public Potion truthPotion;

    public UnityEvent OnConvoStarted;
    public UnityEvent OnBookOpened;
    public UnityEvent OnWin;
    public UnityEvent OnLose;

    public UnityEvent OnPickup;
    public UnityEvent OnFill;
    public UnityEvent OnPour;
    public UnityEvent OnSetDown;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) {
            DoAction();
        }   
    }

    private void DoAction() {
        if (TryTalk()) {
            return;
        }
        if(TryBook()) {
            return;
        }
        if (heldObject != null) {
            if (TryFill()) {
                return;
            }
            if(TryCauldron()) {
                return;
            }
            if(TryPutDown()) {
                return;
            }
        } else {
            TryPickup();
        }
    }

    private bool TryBook() {
        foreach (Collider2D hit in GetCollidersFacing()) {
            Book b = hit.GetComponent<Book>();
            if (b != null) {
                OnBookOpened.Invoke();
                return true;
            }
        }
        return false;
    }

    private bool TryTalk() {
        foreach (Collider2D hit in GetCollidersFacing()) {
            Leogan l = hit.GetComponent<Leogan>();
            if (l != null) {
                if (heldObject != null) {
                    if (heldObject.GetComponent<FlaskContents>().contents == truthPotion) {
                        OnWin.Invoke();
                        OnConvoStarted.Invoke();
                    }
                    if (heldObject.GetComponent<FlaskContents>().contents == antidotePotion) {
                        OnLose.Invoke();
                        OnConvoStarted.Invoke();
                    }
                } else {
                    OnConvoStarted.Invoke();
                }
                return true;
            }
        }
        return false;
    }

    private bool TryCauldron() {
        if (heldObject.GetComponent<FlaskContents>().contents == null) {
            return false;
        }

        foreach (Collider2D hit in GetCollidersFacing()) {
            Cauldron cauldron = hit.GetComponent<Cauldron>();
            if (cauldron != null) {
                if (cauldron.acceptingContents) {
                    heldObject.isCarried = false;
                    StartCoroutine(FlaskToCauldron(cauldron, heldObject));
                    heldObject = null;
                    OnPour.Invoke();
                }
                return true;
            }
        }
        return false;
    }

    private IEnumerator FlaskToCauldron(Cauldron cauldron, CarryEnabler c) {
        cauldron.TakePotion(c.GetComponent<FlaskContents>());
        Destroy(c.gameObject);
        yield break;
    }

    private bool TryFill() {
        foreach(Collider2D hit in GetCollidersFacing()) {
            FlaskFiller filler = hit.GetComponent<FlaskFiller>();
            if(filler != null) {
                if (heldObject.GetComponent<FlaskContents>().contents == null) {
                    heldObject.GetComponent<FlaskContents>().contents = filler.potion;
                    OnFill.Invoke();
                }
                return true;
            }
        }
        return false;
    }

    private List<Collider2D> GetCollidersFacing() {
        ContactFilter2D filter = new ContactFilter2D();
        filter.NoFilter();

        List<Collider2D> hits = new List<Collider2D>();
        pickupCollider.OverlapCollider(filter, hits);

        return hits;
    }

    private bool TryPutDown() {
        List<PlacementPad> possiblePlacements = new List<PlacementPad>();

        bool foundPlacement = false;

        foreach (Collider2D hit in GetCollidersFacing()) {
            PlacementPad placeable = hit.GetComponent<PlacementPad>();
            if (placeable != null) {
                foundPlacement = true;
                if (placeable.CanPlace(heldObject)) {
                    possiblePlacements.Add(placeable);
                }
            }
        }

        if (possiblePlacements.Count > 0) {
            PlacementPad closest = null;
            float closestDistance = Mathf.Infinity;
            foreach (PlacementPad pad in possiblePlacements) {
                float distance = Vector3.Distance(pad.transform.position, holdSpot.transform.position);
                if (distance < closestDistance) {
                    closestDistance = distance;
                    closest = pad;
                }
            }

            closest.Place(heldObject);
            OnSetDown.Invoke();
            heldObject.isCarried = false;
            heldObject = null;
        } else {
            if (!foundPlacement) {
                return true;
                heldObject.Throw(transform.up);
                heldObject.isCarried = false;
                heldObject = null;
            }
        }

        return true;
    }

    private void TryPickup() {
        ContactFilter2D filter = new ContactFilter2D();
        filter.NoFilter();

        List<Collider2D> hits = new List<Collider2D>();
        pickupCollider.OverlapCollider(filter, hits);

        foreach (Collider2D hit in hits) {
            CarryEnabler carriable = hit.GetComponent<CarryEnabler>();
            if (carriable != null && carriable.canPickUp) {
                Pickup(carriable);
                OnPickup.Invoke();
                return;
            }
        }
    }

    private void Pickup(CarryEnabler carriable) {
        carriable.transform.parent = holdSpot;
        carriable.transform.localEulerAngles = Vector3.zero;
        carriable.transform.localPosition = Vector3.up * holdOffset;

        carriable.isCarried = true;

        heldObject = carriable;
    }
}
