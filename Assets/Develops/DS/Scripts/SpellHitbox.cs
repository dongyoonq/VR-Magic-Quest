using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MonsterSkillData;

public class SpellHitbox : MonoBehaviour
{
    private Spell spell;
    private Collider hitCollider;
    public Collider HitCollider { get { return hitCollider; } }
    private float time;
    private bool hittable;
    // TODO: 연속데미지

    public void SynchronizeSpell(Spell spell)
    {
        this.spell = spell;
    }

    private void Awake()
    {
        hitCollider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        time = 0f;
        hittable = true;
        hitCollider.enabled = false;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (spell != null)
        {
            spell.Hit(transform.position);
        }  
    }

    private void OnTriggerEnter(Collider other)
    {
        time = 0f;
    }

    private void OnTriggerStay(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
