using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CauldronContent;

public class box : MonoBehaviour
{
    [SerializeField] public GameObject openobj;
    [SerializeField] private Transform itempos;

    public void Open()
    {
        openobj.transform.Rotate(-50f, 0, 0);
     GameObject obj= GameManager.Resource.Instantiate<GameObject>($"Prefabs/Recipe/Gravity Rock Recipe", itempos.position, Quaternion.identity);
        GameObject obj2=GameManager.Resource.Instantiate<GameObject>($"Prefabs/Recipe/Gravity Distortion Recipe", itempos.position, Quaternion.identity);
        GameManager.Sound.PlaySFX("boxopen");
    }
}
