using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeSlotUI : MonoBehaviour
{
    public PortionRecipeData recipeData;
    public Image recipeImage;
    public Image frameImage;
    private RecipeUI recipeUI;

    private void Start()
    {
        if (recipeData != null) 
            recipeImage.sprite = recipeData.RecipeSprite;

        recipeUI = GetComponentInParent<RecipeUI>();

        GetComponent<Button>().onClick.AddListener(() => ClickButton());
    }

    private void OnEnable()
    {
        recipeImage = transform.GetChild(0).GetComponent<Image>();
        frameImage = transform.GetChild(1).GetComponent<Image>();
    }

    public void ClickButton()
    {
        if (recipeData != null) 
            recipeUI.ViewRecipe(recipeData);
    }
}
