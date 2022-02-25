using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{
    public int energy;
    public bool isBait;

    public void Eat() {
        Destroy(gameObject);
    }
}