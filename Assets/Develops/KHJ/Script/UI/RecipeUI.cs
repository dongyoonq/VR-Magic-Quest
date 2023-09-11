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

            foreach(PortionRecipeData portionRecipeData in recipeManager.portionRecipes)
            {
                if (portionRecipeData == slot.recipeData)
                {
                    if (portionRecipeData.isUnlock)
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

    public void ViewRecipe(PortionRecipeData recipeData)
    {
        foreach(PortionRecipeData portionRecipeData in recipeManager.portionRecipes)
        {
            if (portionRecipeData == recipeData)
            {
                if (portionRecipeData.isUnlock)
                {
                    runeImage1.sprite = portionRecipeData.ingredientRune1.IconSprite;
                    runeImage2.sprite = portionRecipeData.ingredientRune2.IconSprite;
                    runeImage3.sprite = portionRecipeData.ingredientRune3.IconSprite;
                    portionImage.sprite = portionRecipeData.madePortion.IconSprite;
                    runeName1.SetText(portionRecipeData.ingredientRune1.Name);
                    runeName2.SetText(portionRecipeData.ingredientRune2.Name);
                    runeName3.SetText(portionRecipeData.ingredientRune3.Name);
                    PortionName.SetText(portionRecipeData.madePortion.Name);
                }
                else
                {
                    runeImage1.sprite = null;
                    runeImage2.sprite = null;
                    runeImage3.sprite = null;
                    runeName1.SetText("");
                    runeName2.SetText("");
                    runeName3.SetText("");
                    portionImage.sprite = portionRecipeData.madePortion.IconSprite;
                    PortionName.SetText(portionRecipeData.madePortion.Name);
                }
            }
        }
    }
}
