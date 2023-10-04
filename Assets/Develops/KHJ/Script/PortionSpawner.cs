using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �����͸� ���� ������ �ְ� �ش� ���� �����ǿ� �� �ִ�
/// �̸��� ���� ���ǵ������� �������� ���������ش�.
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
