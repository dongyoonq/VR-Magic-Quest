using UnityEngine;

[CreateAssetMenu(fileName = "Skill Data", menuName = "Scriptable Object/Skill Data")]
public class SkillData : ScriptableObject
{
    // ��ų ������ �ٸ� Ŭ�������� ��ȯ ����
    [Tooltip("��ų ������")]
    [SerializeField] public Sprite skillSprite;
    [SerializeField] public Skill skillPrefab;
    [SerializeField] public string skillName;
    [SerializeField, TextArea] public string skillTooltip;
    [SerializeField] public string recognizeGestureName;
    [SerializeField] public int damage;
    [SerializeField] public int useMp;
    [SerializeField] public SkillType skillType;
    [SerializeField] public HitTag[] hitTags;
}