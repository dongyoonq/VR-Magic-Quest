using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class MonsterVision : MonoBehaviour
{
    [Range(0, 360)]
    [SerializeField]
    private float fieldOfView;
    [SerializeField]
    private MultiAimConstraint headAim;
    [SerializeField]
    private MultiAimConstraint upperBodyAim;
    private MonsterPerception perception;

    private void Awake()
    {
        perception = GetComponent<MonsterPerception>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 시야작업과 플레이어 레이어 구분
        perception.SpotEnemy(other.transform);
    }
}
