using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBag : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private RecipeManager recipeManager;

    Player player;

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enter something");
        Item item = other?.GetComponent<Item>();
        Recipe recipe = other?.GetComponent<Recipe>();

        if (item != null)
        {
            Debug.Log(item);
            Debug.Log(item.Data);
            player.AddItemToInventory(item.Data);
            GameManager.Resource.Destroy(item.gameObject);
        }
        else if (recipe != null)
        {
            recipeManager.GetRecipe(recipe.recipeName);
            GameManager.Resource.Destroy(recipe.gameObject);
        }
    }
}
