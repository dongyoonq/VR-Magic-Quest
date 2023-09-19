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
    public GameObject defaultSkillPrefab;

    [Serializable]
    public class SkillInfo
    {
        public string skillName;
        public GameObject skillPrefab;
        public HitTag[] hitType;
        public float damageMultiplier;
        public float castingTime;
        public ActivateTiming activateTiming;
        public float delayTime;
        public float spellDuration;
        public float hitRange;
        public Aim aim;
        public SpellType spellType;
        public SkillInfo[] additionalSkills;
    }
    
    public SkillInfo GetSkillInfo(MonsterSkill skill)
    {
        switch (skill)
        {
            case MonsterSkill.RockShower:
                return this.skill[0];
            case MonsterSkill.Rage:
                return this.skill[1];
            case MonsterSkill.RootBind:
                return this.skill[2];
            case MonsterSkill.IceShield:
                return this.skill[3];
            case MonsterSkill.Earthquake:
                return this.skill[4];
            case MonsterSkill.ThunderDragonCannon:
                return this.skill[5];
            case MonsterSkill.FireRain:
                return this.skill[6];
            default:
                return null;
        }
    }
}
