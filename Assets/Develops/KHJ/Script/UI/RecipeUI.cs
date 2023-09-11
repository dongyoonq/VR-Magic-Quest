using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static RecipeManager;

public class RecipeUI : MonoBehaviour
{
    [SerializeField] RecipeManager recipeManager;
    [SerializeField] Image runeImage1;
    [SerializeField] Image runeImage2;
    [SerializeField] Image runeImage3;
    [SerializeField] TMP_Text runeName1;
    [SerializeField] TMP_Text runeName2;
    [SerializeField] TMP_Text runeName3;
    [SerializeField] Image portionImage;
    [SerializeField] TMP_Text PortionName;
    [SerializeField] RecipeSlotUI[] recipeSlots;

    public void OnRecipeUI()
    {
        PlayerActiveRecipeCheck();
    }


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
                        //활성화 색상
                        image.color = new Color(1f, 0.95f, 0f);
                        rect.SetAsFirstSibling();
                    }
                    else
                    {
                        // 비활성화 색상
                        image.color = new Color(0.55f, 0.54f, 0.5f);
                        rect.SetAsLastSibling();
                    }
                }
            }
        }
    }

    public void ViewRecipe(string slotName)
    {
        foreach(PortionRecipe portionRecipe in recipeManager.portionRecipes)
        {
            if (portionRecipe.madePortion.Name == slotName)
            {
                if (portionRecipe.isUnlock)
                {
                    runeImage1.sprite = portionRecipe.ingredientRune1.IconSprite;
                    runeImage2.sprite = portionRecipe.ingredientRune2.IconSprite;
                    runeImage3.sprite = portionRecipe.ingredientRune3.IconSprite;
                    portionImage.sprite = portionRecipe.madePortion.IconSprite;
                    runeName1.SetText(portionRecipe.ingredientRune1.Name);
                    runeName2.SetText(portionRecipe.ingredientRune2.Name);
                    runeName3.SetText(portionRecipe.ingredientRune3.Name);
                    PortionName.SetText(portionRecipe.madePortion.Name);
                }
                else
                {
                    portionImage.sprite = portionRecipe.madePortion.IconSprite;
                    PortionName.SetText(portionRecipe.madePortion.Name);
                }
            }
        }
    }
}
