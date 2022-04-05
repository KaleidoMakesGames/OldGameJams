using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Potion : ScriptableObject {
    public Color potionColor;

    public List<Potion> ingredients;

    public Color friendlyColor {
        get {
            float h, s, v;
            Color.RGBToHSV(potionColor, out h, out s, out v);
            v = Mathf.Clamp(v, 0.5f, 1.0f);
            return Color.HSVToRGB(h, s, v);
        }
    }

    public string FString() {
        return "<color=#" + ColorUtility.ToHtmlStringRGB(friendlyColor) + ">" + name.ToUpper() + "</color>";
    }

    public enum MatchType { None, Partial, Full }

    public MatchType Compare(List<Potion> potions) {
        List<Potion> ingredientsToMatch = new List<Potion>(ingredients);
        foreach (Potion p in potions) {
            if (!ingredientsToMatch.Remove(p)) {
                // Couldn't find...
                return MatchType.None;
            }
        }

        if (ingredientsToMatch.Count == 0) {
            return MatchType.Full;
        } else {
            return MatchType.Partial;
        }
    }
}