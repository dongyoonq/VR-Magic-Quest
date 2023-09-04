using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IHittable
{
    [SerializeField] public List<SkillData> skillList;
    [SerializeField] public int hp;

    [NonSerialized] public bool isSkillUsed;

    public void TakeDamaged(int damage)
    {
        hp -= damage;
    }
}