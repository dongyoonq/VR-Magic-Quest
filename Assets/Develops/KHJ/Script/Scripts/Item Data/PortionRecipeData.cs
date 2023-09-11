using UnityEngine;

[CreateAssetMenu(fileName = "Item_Portion_Recipe", menuName = "Inventory System/Item Data/Portion Recipe", order = 3)]
public class PortionRecipeData : ScriptableObject
{
    public Sprite RecipeSprite;
    public RuneItemData ingredientRune1;
    public RuneItemData ingredientRune2;
    public RuneItemData ingredientRune3;
    public PortionItemData madePortion;
    public bool isUnlock = false;
}