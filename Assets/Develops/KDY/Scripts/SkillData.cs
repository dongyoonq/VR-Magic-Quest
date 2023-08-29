using UnityEngine;

[CreateAssetMenu(fileName = "Skill Data", menuName = "Scriptable Object/Skill Data")]
public class SkillData : ScriptableObject
{
    public Sprite skillSprite;
    public Skill skillPrefab;
    public string recognizeGestureName;
}