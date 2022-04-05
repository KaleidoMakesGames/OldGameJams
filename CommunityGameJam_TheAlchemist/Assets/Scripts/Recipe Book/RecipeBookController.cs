using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RecipeBookController : MonoBehaviour
{
    public RecipeBookRenderer left;
    public RecipeBookRenderer right;

    public int recipesPerPage;
    public RecipeBook book;

    private int leftPage;

    public UnityEvent OnBookClose;
    public UnityEvent OnPageTurn;

    private void Awake() {
        leftPage = 0;
    }

    private int maxLeftPage {
        get {
            int maxPage = Mathf.CeilToInt((float)book.potions.Count / (float)recipesPerPage);
            if(maxPage % 2 == 1) {
                return maxPage - 1;
            } else {
                return maxPage;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        left.recipesPerPage = recipesPerPage;
        right.recipesPerPage = recipesPerPage;

        int oldLeft = leftPage;
        if (Input.GetKeyUp(KeyCode.A)) {
            leftPage -= 2;
        }
        if(Input.GetKeyUp(KeyCode.D)) {
            leftPage += 2;
        }

        leftPage = Mathf.Clamp(leftPage, 0, maxLeftPage);

        left.pageNumber = leftPage;
        right.pageNumber = leftPage + 1;

        if (leftPage != oldLeft) {
            left.Redraw();
            right.Redraw();
            OnPageTurn.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            OnBookClose.Invoke();
        }
    }
}
