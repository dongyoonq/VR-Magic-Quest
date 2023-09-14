using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    Player player;

    [SerializeField] Slider hpBar;
    [SerializeField] Slider mpBar;

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }

    private void Update()
    {
        if (hpBar != null)
        {
            hpBar.maxValue = player.maxHp;
            hpBar.value = player.currHp;
        }

        if (mpBar != null)
        {
            mpBar.maxValue = player.maxMp;
            mpBar.value = player.currMp;
        }
    }
}
