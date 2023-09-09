using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �����͸� ���� ������ �ְ� �ش� ���� �����ǿ� �� �ִ�
/// �̸��� ���� ���ǵ������� �������� ���������ش�.
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
