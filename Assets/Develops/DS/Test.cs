using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumType;
using static MonsterSkillData;

public class Test : MonoBehaviour
{
    [SerializeField]
    private GameObject spell1;
    public HitTag[] hitType1;
    public int damage1;
    public float castingTime1;
    public float delayTime1;
    public float spellDuration1;
    public float hitRange1;
    public Aim aim1;
    public SpellType spellType1;
    public Vector3 otherPosition1;
    public Transform targetTransform;

    private bool channelling;
    private Coroutine castingRoutine;

    private void Start()
    {
        StartCoroutine(TestRoutine());
    }

    public void Casting()
    {
        castingRoutine = StartCoroutine(CastingRoutine());
    }

    public IEnumerator CastingRoutine()
    {
        channelling = true;
        yield return new WaitForSeconds(castingTime1);
        channelling = false;
        yield return new WaitForSeconds(delayTime1);
        CastSpell();
    }

    private void CastSpell()
    {
        Vector3 spellPos = Random.insideUnitSphere + targetTransform.position;
        Spell a = GameManager.Resource.Instantiate(spell1, new Vector3(spellPos.x, transform.position.y, spellPos.z), transform.rotation).GetComponent<Spell>();
        
    }

    public void HitReact()
    {
        if (channelling)
        {
            StopCoroutine(CastingRoutine());
        }
    }

    private IEnumerator TestRoutine()
    {
        yield return null;
        Casting();
    }
}
