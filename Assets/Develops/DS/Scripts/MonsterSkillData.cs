using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumType;

[CreateAssetMenu(fileName = "MonsterSkillData", menuName = "Data/Tag")]
public class MonsterSkillData : ScriptableObject
{
    [SerializeField]
    private SkillInfo[] skill;
    public SkillInfo[] Skill { get { return skill; } }

    [Serializable]
    public class SkillInfo
    {
        public string skillName;
        public HitTag[] hitType;
        public float castingTime;
        public float delayTime;
        public Aim aim;
        public SkillType skillType;

        public SkillInfo[] additionalEffects;
    }

    // юс╫ц
    public enum Aim
    {
        Self, // trannsform position
        Front, // ray
        Target // vision.target
    }
    public enum SkillType
    {
        Burst, // overapsphere
        Area, // instantiate, move, Trigger
        Projectile, // instantiate, Trigger
        Falling // random position around position position + Random.insideUnitCircle, move down
    }
}
