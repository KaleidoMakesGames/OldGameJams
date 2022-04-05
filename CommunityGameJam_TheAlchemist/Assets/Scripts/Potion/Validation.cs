using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Validation : MonoBehaviour {
    public RecipeBook book;
    public List<Potion> starting;
    public int level;

    private void OnEnable() {
        Validate();
    }

    private void Validate() {
        List<Potion> producable = new List<Potion>(starting);
        
        for (int i = 0; i < level; i++) {
            foreach (Potion p in book.potions) {
                if (producable.Contains(p)) {
                    continue;
                }
                bool canMake = true;
                foreach (Potion ingredient in p.ingredients) {
                    if (!producable.Contains(ingredient)) {
                        canMake = false;
                        break;
                    }
                }
                if (canMake) {
                    Debug.Log(string.Format("Able to make {0} at level {1}.", p.name, i));
                    producable.Add(p);
                }
            }
        }

        Debug.Log(string.Format("Unable to make {0} potions.", (book.potions.Count - producable.Count)));

        foreach(Potion p in book.potions) {
            foreach(Potion q in book.potions) {
                if (starting.Contains(p) || starting.Contains(q) || p == q) {
                    continue;
                }
                var comp = p.Compare(q.ingredients);
                if(comp != Potion.MatchType.None) {
                    Debug.Log(string.Format("CONFLICT BETWEEN {0} and {1}", p.name, q.name));
                }
            }
        }
    }
}
