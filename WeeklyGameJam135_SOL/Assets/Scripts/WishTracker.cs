using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WishTracker : MonoBehaviour
{
    public UnityEvent OnWishGain;

    public int currentNumberOfWishes;

    public void AddWish() {
        currentNumberOfWishes++;
        OnWishGain.Invoke();
    }

    public void UseWish() {
        currentNumberOfWishes--;
    }
}
