using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public struct ToSpawn {
        public float probability;
        public float distanceFromStartMultiplier;
    }
}
