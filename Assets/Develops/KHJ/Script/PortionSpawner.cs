using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 포션 데이터를 전부 가지고 있고 해당 받은 레시피에 들어가 있는
/// 이름과 같은 포션데이터의 프리팹을 스폰시켜준다.
/// </summary>
public class PortionSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPostion;
    [SerializeField] private GameObject failPostion;

    public void PotionBrewed(PortionRecipeData potionData)
    {
        if (potionData == null)
        {
            Instantiate(failPostion, spawnPostion.position, transform.rotation);
        }
        else
        {
            Instantiate(potionData.madePortion.DropItemPrefab, spawnPostion.position, transform.rotation);
        }
    }
}
