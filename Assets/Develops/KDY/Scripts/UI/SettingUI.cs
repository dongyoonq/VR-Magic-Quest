using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUI : MonoBehaviour
{
    [SerializeField] MagicWand wand;

    public void CreateWand()
    {
        Instantiate(wand, Camera.main.transform.position + (Camera.main.transform.forward * 2f), Quaternion.identity);
    }
}
