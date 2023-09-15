using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumType;
using static MonsterSkillData;

[CreateAssetMenu(fileName = "MonsterSkillData", menuName = "Data/MonsterSkillData")]
public class MonsterSkillData : ScriptableObject
{
    [SerializeField]
    private SkillInfo[] skill;
    public SkillInfo[] Skill { get { return skill; } }

    [Serializable]
    public class SkillInfo
    {
        public string skillName;
        public GameObject skillPrefab;
        public HitTag[] hitType;
        public int damage;
        public float castingTime;
        public float delayTime;
        public float spellDuration;
        public float hitRange;
        public Aim aim;
        public SpellType spellType;
        public SkillInfo[] additionalEffects;
        public Vector3 otherPosition;

        public void GetOtherPosition(Vector3 position)
        {
            otherPosition = position;
        }
    }

    public void CastSpell(SkillInfo skillInfo, MonsterPerception caster)
    {
        caster.StartCoroutine(CastSpellRoutine(skillInfo, caster));
    }

    private IEnumerator CastSpellRoutine(SkillInfo skillInfo, MonsterPerception caster)
    {
        Spell spell;
        switch (skillInfo.aim)
        {
            case Aim.Self:
                spell = GameManager.Resource.Instantiate
                    (skillInfo.skillPrefab, caster.transform.position, Quaternion.identity, true).GetComponent<Spell>();
                spell.SynchronizeSpell(skillInfo);
                break;
            case Aim.Front:
                spell = GameManager.Resource.Instantiate
                    (skillInfo.skillPrefab, caster.transform.position + caster.transform.forward, Quaternion.identity, true)
                    .GetComponent<Spell>();
                spell.SynchronizeSpell(skillInfo);
                break;
            case Aim.Target:
                spell = GameManager.Resource.Instantiate
                    (skillInfo.skillPrefab, caster.Vision.TargetTransform.position, Quaternion.identity, true)
                    .GetComponent<Spell>();
                spell.SynchronizeSpell(skillInfo);
                break;
            default:
                spell = GameManager.Resource.Instantiate
                    (skillInfo.skillPrefab, skillInfo.otherPosition, Quaternion.identity, true).GetComponent<Spell>();
                spell.SynchronizeSpell(skillInfo);
                break;
        }
        if (skillInfo.additionalEffects.Length > 0)
        {
            foreach (SkillInfo additionalSkill in skillInfo.additionalEffects)
            {
                yield return new WaitWhile(() => skillInfo.otherPosition == null);
                CastSpell(additionalSkill, caster);
            }
        }
        yield return null;
    }

    // юс╫ц
    //public enum Aim
    //{
    //    Self, // trannsform position
    //    Front, // ray
    //    Target // vision.target
    //}
    //public enum SkillType
    //{
    //    Burst, // overapsphere
    //    Area, // instantiate, move, Trigger
    //    Projectile, // instantiate, Trigger
    //    Falling // random position around position position + Random.insideUnitCircle, move down
    //}
}
