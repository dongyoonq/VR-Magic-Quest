using UnityEngine;

public class TeleportSkill : Skill
{
    public override void CastingSpell(Player player, float distanceValue, Transform createTrans)
    {
        //GameManager.Resource.Instantiate(skillData.skillPrefab, true);

        Teleport(player, distanceValue);
    }

    public void Teleport(Player player, float distance)
    {
        CharacterController controller = player.GetComponent<CharacterController>();

        controller.Move(Camera.main.transform.forward * AdjustDistance(distance));
    }

    private float AdjustDistance(float distance)
    {
        float result = distance * 10;

        if (result < 0)
            result = 0f;

        Debug.Log(result);

        return result;
    }
}