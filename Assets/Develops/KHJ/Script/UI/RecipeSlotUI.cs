using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeSlotUI : MonoBehaviour
{
    public Image frameImage;
    public string portionName;
    private RecipeUI recipeUI;

    public void ClickButton()
    {
        recipeUI.ViewRecipe(portionName);
    }
}
