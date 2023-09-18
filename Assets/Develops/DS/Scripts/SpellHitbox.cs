using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MonsterSkillData;

public class SpellHitbox : MonoBehaviour
{
    private Spell spell;
    private float time;
    private bool hittable;
    // TODO: 연속데미지

    public void SynchronizeSpell(Spell spell)
    {
        this.spell = spell;
    }

    private void OnEnable()
    {
        time = 0f;
        hittable = true;
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
            spell.Hit(transform.position);
        }  
    }

    private void OnTriggerStay(Collider other)
    {
        spell.ContinuousHit(other);
    }
}
