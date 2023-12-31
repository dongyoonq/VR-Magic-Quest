using PDollarGestureRecognizer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using static EnumType;

public class Player : MonoBehaviour, IHittable, IHitReactor
{
    public UnityEvent<ItemData, int, int> OnAddItemInventory;
    public UnityEvent<ItemData, int, int> OnRemoveItemInventory;

    public InventoryUI inventoryUI { get; set; }
    public Inventory inventory { get; private set; }
    public SettingUI settingUI;

    public List<SkillData> skillList;
    public List<PortionRecipeData> unlockRecipeList;
    public List<Gesture> trainingSet = new List<Gesture>();

    [SerializeField] Image hitScreen;

    [SerializeField] public int maxHp;
    [SerializeField] public int maxMp;
    [SerializeField] public ActionBasedControllerManager controllerManager;
    
    public int currHp;
    public int currMp;

    private LocomotionSystem playerLocomotion;

    [NonSerialized] public bool isSkillUsed;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        playerLocomotion = GetComponentInChildren<LocomotionSystem>();
    }

    private void Start()
    {
        currHp = maxHp;
        currMp = maxMp;

        trainingSet = GameManager.Load.LoadGestures();
    }   

    private void Update()
    {

    }

    // 인벤토리 추가 메서드
    public void AddItemToInventory(ItemData item)
    {
        if (inventory.list.Count(x => x == null) == 0)
        {
            Debug.Log("가방이 가득찼다");
            return;
        }

        int index = -1;
        if (item is CountableItemData)
        {
            index = inventory.list.FindIndex(x => x == item);

            if (index == -1)
            {
                for (int i = 0; i < inventory.list.Count; i++)
                {
                    if (inventory.list[i] == null)
                    {
                        inventory.list[i] = item;
                        inventoryUI.slots[i].slotIndex = i;
                        index = i;
                        break;
                    }
                }
            }

            OnAddItemInventory?.Invoke(item, index, 1);
            GameManager.Quest.GatherItem(item);
            return;
        }

        for (int i = 0; i < inventory.list.Count; i++)
        {
            if (inventory.list[i] == null)
            {
                inventory.list[i] = item;
                inventoryUI.slots[i].slotIndex = i;
                index = i;
                break;
            }
        }

        OnAddItemInventory?.Invoke(item, index, 1);
        GameManager.Quest.GatherItem(item);
    }

    public void AddItemToInventory(ItemData item, int index = 0)
    {
        OnAddItemInventory?.Invoke(item, index, 1);
    }

    // 인벤토리 아이템 제거 메서드
    public void RemoveItemFromInventory(ItemData item, int index = -1)
    {
        if (index == -1)
        {
            for (int i = 0; i < inventory.list.Count; i++)
            {
                if (inventory.list[i] == item)
                {
                    index = i;
                    break;
                }
            }
        }

        OnRemoveItemInventory?.Invoke(item, index, 1);

        if (item is not CountableItemData)
        {
            inventory.list[index] = null;
        }
        else if (item is CountableItemData)
        {
            if (inventoryUI.slots[index].amount == 0)
                inventory.list[index] = null;
        }
    }

    public void UnlockRecipe(PortionRecipeData data)
    {
        if (!unlockRecipeList.Contains(data))
            unlockRecipeList.Add(data);
    }

    public void LockRecipe(PortionRecipeData data)
    {
        if (unlockRecipeList.Contains(data))
            unlockRecipeList.Remove(data);
    }

    public void HitReact(HitTag[] hitType, float duration)
    {
        foreach (HitTag hitTag in hitType)
        {
            switch (hitTag)
            {
                case HitTag.Impact:
                    break;
                case HitTag.DebuffSpeed:
                    break;
                case HitTag.BuffSpeed:
                    break;
                case HitTag.Mez:
                    StartCoroutine(MezRoutine(duration));
                    break;
            }
        }
    }

    public void TakeDamaged(int damage)
    {
        currHp -= damage;

        StartCoroutine(GotHurtRoutine());

        if (currHp < 0)
        {
            StartCoroutine(DieRoutine());
        }
    }

    IEnumerator DieRoutine()
    {
        CharacterController controller = GetComponent<CharacterController>();
        controller.enabled = false;

        yield return new WaitForSeconds(1f);
        GameManager.Sound.PlaySFX("PlayerDie");
        settingUI.ReturnHome();
        yield return new WaitForSeconds(1f);

        controller.enabled = true;
        currHp = maxHp;
    }

    IEnumerator GotHurtRoutine()
    {
        Color startColor = hitScreen.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0.7f);

        float time = 0f;
        float duration = 0.3f;

        while (time < duration)
        {
            hitScreen.color = Color.Lerp(startColor, targetColor, time / duration);
            time += Time.deltaTime;

            yield return null;
        }

        time = 0f;
        duration = 0.7f;

        startColor = hitScreen.color;
        targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (time < duration)
        {
            hitScreen.color = Color.Lerp(startColor, targetColor, time / duration);
            time += Time.deltaTime;

            yield return null;
        }
    }

    public void ActiveLocomotion(bool active)
    {
        playerLocomotion.gameObject.SetActive(active);
    }

    IEnumerator MezRoutine(float duration)
    {
        isSkillUsed = true;
        ActiveLocomotion(false);

        yield return new WaitForSeconds(duration);

        isSkillUsed = false;
        ActiveLocomotion(true);
    }

    public void PlayerEndBattle()
    {
        MonsterController[] monsters = GetComponentsInChildren<MonsterController>();

        if (monsters.Length == 0)
        {
            GameManager.Sound.PlayBGM("DungeonBGM");
        }
    }
}