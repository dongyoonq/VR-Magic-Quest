using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RecipeUI : BookUI
{
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
        recipeSlots = GetComponentsInChildren<RecipeSlotUI>();
        PlayerActiveRecipeCheck();
    }

    private void PlayerActiveRecipeCheck()
    {
        foreach (RecipeSlotUI slot in recipeSlots)
        {
            Image image = slot.frameImage;
            RectTransform rect = slot.GetComponent<RectTransform>();

            if (player.unlockRecipeList.Contains(slot.recipeData))
            {
                // 활성화 색상
                image.color = new Color(1f, 1f, 0f);
                rect.SetAsFirstSibling();
            }
            else
            {
                // 비활성화 색상
                image.color = new Color(0.75f, 0.67f, 0.67f);
                rect.SetAsLastSibling();
            }
        }
    }

    public void ViewRecipe(PortionRecipeData recipeData)
    {
        if (player.unlockRecipeList.Contains(recipeData))
        {
            runeImage1.sprite = recipeData.ingredientRune1.IconSprite;
            runeImage2.sprite = recipeData.ingredientRune2.IconSprite;
            runeImage3.sprite = recipeData.ingredientRune3.IconSprite;
            portionImage.sprite = recipeData.madePortion.IconSprite;
            runeName1.SetText(recipeData.ingredientRune1.Name);
            runeName2.SetText(recipeData.ingredientRune2.Name);
            runeName3.SetText(recipeData.ingredientRune3.Name);
            PortionName.SetText(recipeData.madePortion.Name);
        }
        else
        {
            runeImage1.sprite = null;
            runeImage2.sprite = null;
            runeImage3.sprite = null;
            runeName1.SetText("");
            runeName2.SetText("");
            runeName3.SetText("");
            portionImage.sprite = recipeData.madePortion.IconSprite;
            PortionName.SetText(recipeData.madePortion.Name);
        }
    }
}
