using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    [SerializeField] public PortionRecipeData[] portionRecipes;

    public void GetRecipe(PortionRecipeData recipeData)
    {
        recipeData.isUnlock = true;
    }
}
