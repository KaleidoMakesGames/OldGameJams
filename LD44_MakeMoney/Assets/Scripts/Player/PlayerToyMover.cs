using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PlayerToyMover : MonoBehaviour
{
    public Camera gameCamera;

    public UnityEvent OnUp;
    public UnityEvent OnDown;

    [System.Serializable]
    public class BadDropEvent : UnityEvent<ToyController> { }

    public BadDropEvent OnBadDrop;

    public ToyController heldToy {
        get; private set;
    }

    private void Awake() {
        heldToy = null;
    }

    private void Update() {
        if(EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
        if (heldToy == null) {
            CheckForToyPickup();
            CheckForSpeedChange();
        } else {
            UpdateHeldToy();
            CheckForToyPlacement();
        }
    }

    public void DropHeld() {
        if(heldToy != null) {
            Destroy(heldToy.gameObject);
            heldToy = null;
        }
    }

    private void CheckForSpeedChange() {
        if (heldToy == null) {
            if (Input.GetMouseButtonUp(0)) {
                foreach (RaycastHit2D hit in Physics2D.RaycastAll(gameCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero)) {
                    BeltSpeedChanger changer = hit.collider.GetComponent<BeltSpeedChanger>();
                    if (changer!= null) {
                        changer.DoChange();
                    }
                }
            }
        }
    }

    private void CheckForToyPickup() {
        ToyController foundToy = null;
        if (Input.GetMouseButtonDown(0)) {
            foreach (RaycastHit2D hit in Physics2D.RaycastAll(gameCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero)) {
                if(hit.collider.GetComponent<ToyPutDownRegion>() != null && !hit.collider.GetComponent<ToyPutDownRegion>().canPutDownHere) {
                    return;
                }
                ToyController controller = hit.collider.GetComponent<ToyController>();
                if (controller != null) {
                    foundToy = controller;
                }
            }
        }
        if(foundToy != null) {
            heldToy = foundToy;
            heldToy.PickUp();
            OnUp.Invoke();
            return;
        }
    }

    private void UpdateHeldToy() {
        heldToy.transform.position = (Vector2)gameCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void CheckForToyPlacement() {
        if (Input.GetMouseButtonUp(0)) {
            if (heldToy.canPutDown) {
                heldToy.PutDown();
                OnDown.Invoke();
                heldToy = null;
            } else {
                OnBadDrop.Invoke(heldToy);
                heldToy = null;
            }
        }
    }
}
