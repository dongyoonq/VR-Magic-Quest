public class EnumType
{
    public enum SkillType
    {
        Fire,
        IceLightning,
        Venom,
        Gravity,
        Teleport,
    }

    public enum HitTag
    {
        Impact,
        Buff,
        Debuff,
        Mez
    }

    public enum MonsterTag
    {
        Guard,
        Tenacity,
        Aggresive,
        DynamicallyMove,
        Cautious,
        SpellCaster,
        Agile,
        Gimmick,
        Elite,
        LastBoss
    }

    public enum State
    {
        Idle,
        Alert,
        Chase,
        Combat,
        Collapse
    }

    public enum Condition
    {
        Weak,
        Exhausted,
        Fatigued,
        Tired,
        Normal,
        Refreshing,
        Good,
        Energetic,
        TopForm
    }

    public enum MonsterSkill
    {
        Basic = -1,
        RockShower,
        Rage,
        RootBind,
        IceShield,
        Earthquake,
        ThunderDragonCannon,
        FireRain
    }

    public enum Aim
    {
        Self,
        Front,
        Target,
        Around
    }

    public enum SpellType
    {
        Burst,
        Area, 
        Projectile,
    }

    public enum ActivateTiming
    {
        After,
        DelayTime,
        Immediately
    }
}