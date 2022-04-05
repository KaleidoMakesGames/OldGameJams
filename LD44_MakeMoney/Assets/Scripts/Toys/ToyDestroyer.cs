using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyDestroyer : MonoBehaviour
{
    public void DestroyToy(ToyController toy) {
        StartCoroutine(DestroyLate(toy.gameObject));
    }

    private IEnumerator DestroyLate(GameObject toyObject) {
        yield return new WaitForEndOfFrame();
        Destroy(toyObject);
    }
}
