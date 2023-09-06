using UnityEngine;

[CreateAssetMenu(fileName = "Skill Data", menuName = "Scriptable Object/Skill Data")]
public class SkillData : ScriptableObject
{
    // 스킬 데이터 다른 클래스에서 변환 주의
    [Tooltip("스킬 데이터")]
    [SerializeField] public Sprite skillSprite;
    [SerializeField] public Skill skillPrefab;
    [SerializeField] public string skillName;
    [SerializeField, TextArea] public string skillTooltip;
    [SerializeField] public string recognizeGestureName;
    [SerializeField] public int damage;
}