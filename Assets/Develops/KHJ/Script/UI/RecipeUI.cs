using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static RecipeManager;

public class RecipeUI : MonoBehaviour
{
    [SerializeField] RecipeManager recipeManager;
    [SerializeField] RecipeSlotUI[] recipeSlots;
    public UnityEvent OnPlayerRecipeUIUpdate;


    private void PlayerActiveRecipeCheck()
    {
        foreach (RecipeSlotUI slot in recipeSlots)
        {
            Image image = slot.frameImage;
            RectTransform rect = slot.GetComponent<RectTransform>();

            foreach(PortionRecipe portionRecipe in recipeManager.portionRecipes)
            {
                if (portionRecipe.madePortion.Name == slot.portionName)
                {
                    if (portionRecipe.isUnlock)
                    {
                        //Ȱ��ȭ ����
                        image.color = new Color(1f, 0.95f, 0f);
                        rect.SetAsFirstSibling();
                    }
                    else
                    {
                        // ��Ȱ��ȭ ����
                        image.color = new Color(0.55f, 0.54f, 0.5f);
                        rect.SetAsLastSibling();
                    }
                }
            }
        }
    }
}
