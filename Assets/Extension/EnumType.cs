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
        Melee,
        LongRange,
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
}