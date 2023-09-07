using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;

public class ExclusiveRayInteractor : XRRayInteractor
{
    bool OnPointer = false;

    public override void GetValidTargets(List<IXRInteractable> targets)
    {
        base.GetValidTargets(targets);

        if (TryGetCurrentUIRaycastResult(out RaycastResult result))
        {
            SkillSlot slot = result.gameObject.GetComponent<SkillSlot>();

            if (slot != null && !OnPointer)
            {
                SkillUI skillUI = slot.GetComponentInParent<SkillUI>();
                skillUI.OnPointerSkllSlot?.Invoke(slot);

                OnPointer = true;
            }
            else
            {
                OnPointer = false;
            }
        }
    }
}
