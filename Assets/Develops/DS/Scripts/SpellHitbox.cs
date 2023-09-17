using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellHitbox : MonoBehaviour
{
    private Spell spell;
    private float time;
    private Coroutine continuousHitCheckRoutine;

    public void SynchronizeSpell(Spell spell)
    {
        this.spell = spell;
    }

    private IEnumerator ContinuousHitCheckRoutine()
    {
        while (true)
        {            
            if (time <= 0f)
            {
                spell.Hit(transform.position);
                time = 0.1f;
            }
            else
            {
                time += -Time.deltaTime;
            }
            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        spell.Hit(collision.contacts[0].point);
    }

    private void OnTriggerEnter(Collider other)
    {
        continuousHitCheckRoutine = StartCoroutine(ContinuousHitCheckRoutine());
    }

    private void OnTriggerExit(Collider other)
    {
        StopCoroutine(continuousHitCheckRoutine);
    }
}
