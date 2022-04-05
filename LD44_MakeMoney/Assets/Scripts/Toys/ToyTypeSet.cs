using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Make Money/Toy Type Set")]
public class ToyTypeSet : ScriptableObject
{
    [System.Serializable]
    public struct ToyElement {
        public ToyType toyType;
        public int probabilityWeight;
    }

    public List<ToyElement> set;

    public ToyType GetRandomToyType() {
        int sum = 0;
        foreach(ToyElement element in set) {
            sum += element.probabilityWeight;
        }

        int random = Random.Range(0, sum);
        int currentLow = 0;

        foreach (ToyElement element in set) { 
            int currentHigh = currentLow + element.probabilityWeight;
            if(random >= currentLow && random < currentHigh) {
                return element.toyType;
            } else {
                currentLow = currentHigh;
            }
        }

        return null;
    }
}
