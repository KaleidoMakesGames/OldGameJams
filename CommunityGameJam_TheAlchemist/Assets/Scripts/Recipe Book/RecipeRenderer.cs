using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeRenderer : MonoBehaviour
{
    public Potion potion;
    public PotionRenderer potionPrefab;
    public Transform ingredientsTransform;
    public Transform resultTransform;

    private void Start() {
        foreach(Potion ingredient in potion.ingredients) {
            PotionRenderer newPotionRenderer = Instantiate(potionPrefab.gameObject, ingredientsTransform).GetComponent<PotionRenderer>();
            newPotionRenderer.potion = ingredient;
        }

        PotionRenderer result = Instantiate(potionPrefab.gameObject, resultTransform).GetComponent<PotionRenderer>();
        result.potion = potion;
    }
}
