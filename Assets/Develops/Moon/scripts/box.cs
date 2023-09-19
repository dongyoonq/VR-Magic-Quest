using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CauldronContent;

public class box : MonoBehaviour
{
    [SerializeField] public GameObject openobj;
    [SerializeField] private Transform itempos;

    [SerializeField] private List<Recipe> recipes;

    public void Open()
    {
        openobj.transform.Rotate(-50f, 0, 0);
        int random = Random.Range(0, recipes.Count);
        GameManager.Resource.Instantiate(recipes[random], itempos.position, Quaternion.identity);

        GameManager.Sound.PlaySFX("boxopen");
    }
}
