using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RecipeBook : ScriptableObject
{
    public List<Potion> potions;

    public List<Potion> GetSorted() {
        List<Potion> r = new List<Potion>(potions);
        r.Sort((x, y) => x.name.CompareTo(y.name));
        return r;
    }
}
