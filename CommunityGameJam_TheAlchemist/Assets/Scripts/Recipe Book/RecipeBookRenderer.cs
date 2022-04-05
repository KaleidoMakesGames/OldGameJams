using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecipeBookRenderer : MonoBehaviour
{
    public RecipeBook book;
    public int pageNumber;
    public int recipesPerPage;
    public TextMeshProUGUI pageTextField;


    public RecipeRenderer recipeRendererPrefab;

    public Transform container;

    private void Start() {
        Redraw();
    }

    public void Redraw() {
        List<Potion> potions = book.GetSorted();

        foreach(Transform t in container) {
            Destroy(t.gameObject);
        }

        for(int i = 0; i < recipesPerPage; i++) {
            int index = i + pageNumber * recipesPerPage;
            if(index >= potions.Count) {
                break;
            }
            Potion r = potions[index];

            RecipeRenderer newRenderer = Instantiate(recipeRendererPrefab, container).GetComponent<RecipeRenderer>();
            newRenderer.potion = r;
        }

        pageTextField.text = (pageNumber+1).ToString();
    }
}
