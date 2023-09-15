using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumType;
using static MonsterSkillData;

public class Spell : MonoBehaviour
{
    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public Vector3 SynchronizeSpell(SkillInfo skillInfo)
    {
        switch (skillInfo.spellType)
        {
            case SpellType.Area:

                break;
            case SpellType.Projectile:

                break;
            case SpellType.Falling:

                break;
            default:
                //transform.position;
                break;
        }
        StartCoroutine(SpellRoutine(skillInfo));
        return transform.position;
    }

    public IEnumerator SpellRoutine(SkillInfo skillInfo)
    {
        yield return null;
        // юс╫ц
        skillInfo.GetOtherPosition(transform.position);
    }
}
