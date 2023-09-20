using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MonsterSkillData;

public class SpellHitbox : MonoBehaviour
{
    private Spell spell;

    public void SynchronizeSpell(Spell spell)
    {
        this.spell = spell;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (spell != null)
        {
            spell.activate = false;
            spell.ProjectileHit(transform.position);
        }  
    }

    private void OnTriggerStay(Collider other)
    {
        if (spell != null)
        {
            if (spell.activate)
            {
                spell.ContinuousHit(other);
            }
        }   
    }
}
