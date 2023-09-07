using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class VenomSummon : Skill
{
    public bool isCollision = false;

    private void OnEnable()
    {
        isCollision = false;
    }

    public override void CastingSpell(Player player, float value, Transform createTrans)
    {
        player.isSkillUsed = true;

        createTrans = Camera.main.transform;
        Vector3 createPos = createTrans.position + (createTrans.forward * 2f);
        Skill skill = GameManager.Resource.Instantiate(skillData.skillPrefab, createPos, Quaternion.LookRotation(createTrans.forward), true);

        skill.StartCoroutine(CreateLineRenderRoutine(player, skill));

        skill.StartCoroutine(ProjectileMoveRoutine(player, skill));
    }

    IEnumerator CreateLineRenderRoutine(Player player, Skill skill)
    {
        yield return new WaitForSeconds(0.1f);

        LineRenderer[] l_Renderer = skill.GetComponentsInChildren<LineRenderer>();

        foreach (LineRenderer l in l_Renderer)
        {
            VenomSummonCollider v_collider = l.AddComponent<VenomSummonCollider>();
            v_collider.skillSource = skill as VenomSummon;
            v_collider.source = player;

            SphereCollider collider = l.AddComponent<SphereCollider>();
            collider.isTrigger = true;
            collider.center = new Vector3(0, 0, 0);
            collider.radius = 0.5f;
        }

    }

    IEnumerator ProjectileMoveRoutine(Player player, Skill skill)
    {
        yield return new WaitForSeconds(6f);

        player.isSkillUsed = false;

        if (skill.IsValid())
            GameManager.Resource.Destroy(skill);
    }
}