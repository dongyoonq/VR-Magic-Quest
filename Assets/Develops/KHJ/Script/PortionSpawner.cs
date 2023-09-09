using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 포션 데이터를 전부 가지고 있고 해당 받은 레시피에 들어가 있는
/// 이름과 같은 포션데이터의 프리팹을 스폰시켜준다.
/// </summary>
public class PortionSpawner : MonoBehaviour
{
    public PortionItemData[] Portions;
    
    [SerializeField] private Transform spawnPostion;
    [SerializeField] private GameObject failPostion;

    public void PotionBrewed(AlchemyPot.Recipe recipe)
    {
        if (recipe == null)
        {
            Instantiate(failPostion, spawnPostion.position, transform.rotation);
        }
        else
        {
            foreach (PortionItemData PortionData in Portions)
            {
                if (PortionData.Name == recipe.name)
                {
                    Instantiate(PortionData.DropItemPrefab, spawnPostion.position, transform.rotation);
                }
            }
        }
    }
}
