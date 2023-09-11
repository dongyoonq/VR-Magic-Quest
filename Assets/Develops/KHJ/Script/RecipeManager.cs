using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    [System.Serializable]
    public class PortionRecipe
    {
        public Sprite RecipeSprite;
        public RuneItemData ingredientRune1;
        public RuneItemData ingredientRune2;
        public RuneItemData ingredientRune3;
        public PortionItemData madePortion;
        public bool isUnlock = false;
    }

    [SerializeField] public PortionRecipe[] portionRecipes;

    public void GetRecipe(string recipeName)
    {
        foreach(PortionRecipe portionRecipe in portionRecipes)
        {
            if (recipeName == portionRecipe.madePortion.Name)
            {
                portionRecipe.isUnlock = true;
                break;
            }
        }
    }
}
