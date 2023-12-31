using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class BookCanvasUI : MonoBehaviour
{
    [SerializeField] GameObject book;

    [SerializeField] GameObject inventoryCanvas;
    [SerializeField] GameObject skillCanvas;
    [SerializeField] GameObject recipeCanvas;
    [SerializeField] GameObject questCanvas;
    [SerializeField] GameObject settingCanvas;

    [SerializeField] RectTransform inventoryInfoCanvas;
    [SerializeField] RectTransform skillInfoCanvas;
    [SerializeField] RectTransform recipeInfoCanvas;
    [SerializeField] RectTransform settingInfoCanvas;
    [SerializeField] RectTransform questInfoCanvas;

    [SerializeField] Button inventorySwitchButton;
    [SerializeField] Button skillSwitchButton;
    [SerializeField] Button recipeSwitchButton;
    [SerializeField] Button questSwitchButton;
    [SerializeField] Button settingSwitchButton;

    [SerializeField] Sprite selectButtonSprite;
    [SerializeField] Sprite unselectButtonSprite;
    [SerializeField] TMP_Text CanvasName;

    [SerializeField] SkillUI skillUI;
    [SerializeField] RecipeUI recipeUI;


    enum Canvas { Inventory, Skill, Recipe, Quest, Setting}

    private void Awake()
    {
        inventoryCanvas.SetActive(false);
        skillCanvas.SetActive(false);
        recipeCanvas.SetActive(false);
        questCanvas.SetActive(false);
        settingCanvas.SetActive(false);
        InitButtonListener();
    }

    private void OnEnable()
    {
        inventorySwichButtonClick();
    }

    public void ActiveBookUI(bool active)
    {
        book.SetActive(active);
        book.transform.position = Camera.main.transform.position + (Camera.main.transform.forward * 0.6f);
        Vector3 lookdir = (Camera.main.transform.position - book.transform.position).normalized;
        Quaternion rot = Quaternion.LookRotation(lookdir);
        lookdir = new Vector3(rot.eulerAngles.x + 30f, rot.eulerAngles.y, rot.eulerAngles.z);
        book.transform.rotation = Quaternion.Euler(lookdir);
    }

    private void InitButtonListener()
    {
        inventorySwitchButton.onClick.AddListener(() => inventorySwichButtonClick());
        skillSwitchButton.onClick.AddListener(() => SkillSwichButtonClick());
        recipeSwitchButton.onClick.AddListener(() => RecipeSwichButtonClick());
        questSwitchButton.onClick.AddListener(() => QuestSwichButtonClick());
        settingSwitchButton.onClick.AddListener(() => SettingSwichButtonClick());
    }

    private void inventorySwichButtonClick()
    {
        SetActiveCanvas(Canvas.Inventory);
        inventorySwitchButton.image.sprite = selectButtonSprite;
        skillSwitchButton.image.sprite = unselectButtonSprite;
        recipeSwitchButton.image.sprite = unselectButtonSprite;
        questSwitchButton.image.sprite = unselectButtonSprite;
        settingSwitchButton.image.sprite = unselectButtonSprite;
        CanvasName.SetText("Inventory");
    }

    private void SkillSwichButtonClick()
    {
        SetActiveCanvas(Canvas.Skill);
        inventorySwitchButton.image.sprite = unselectButtonSprite;
        skillSwitchButton.image.sprite = selectButtonSprite;
        recipeSwitchButton.image.sprite = unselectButtonSprite;
        questSwitchButton.image.sprite = unselectButtonSprite;
        settingSwitchButton.image.sprite = unselectButtonSprite;
        CanvasName.SetText("Skill");
        skillUI.ActiveSkillUI();
    }

    private void RecipeSwichButtonClick()
    {
        SetActiveCanvas(Canvas.Recipe);
        inventorySwitchButton.image.sprite = unselectButtonSprite;
        skillSwitchButton.image.sprite = unselectButtonSprite;
        recipeSwitchButton.image.sprite = selectButtonSprite;
        questSwitchButton.image.sprite = unselectButtonSprite;
        settingSwitchButton.image.sprite = unselectButtonSprite;
        CanvasName.SetText("Recipe");
        recipeUI.OnRecipeUI();
    }

    private void QuestSwichButtonClick()
    {
        SetActiveCanvas(Canvas.Quest);
        inventorySwitchButton.image.sprite = unselectButtonSprite;
        skillSwitchButton.image.sprite = unselectButtonSprite;
        recipeSwitchButton.image.sprite = unselectButtonSprite;
        questSwitchButton.image.sprite = selectButtonSprite;
        settingSwitchButton.image.sprite = unselectButtonSprite;
        CanvasName.SetText("Quest");
    }

    private void SettingSwichButtonClick()
    {
        SetActiveCanvas(Canvas.Setting);
        inventorySwitchButton.image.sprite = unselectButtonSprite;
        skillSwitchButton.image.sprite = unselectButtonSprite;
        recipeSwitchButton.image.sprite = unselectButtonSprite;
        questSwitchButton.image.sprite = unselectButtonSprite;
        settingSwitchButton.image.sprite = selectButtonSprite;
        CanvasName.SetText("Setting");
    }

    private void SetActiveCanvas(Canvas canvas)
    {
        if (inventoryCanvas != null && inventoryInfoCanvas != null)
        {
            inventoryCanvas.SetActive(canvas == Canvas.Inventory);
            inventoryInfoCanvas.gameObject.SetActive(canvas == Canvas.Inventory);
        }

        if (skillCanvas != null && skillInfoCanvas != null)
        {
            skillCanvas.SetActive(canvas == Canvas.Skill);
            skillInfoCanvas.gameObject.SetActive(canvas == Canvas.Skill);
        }

        if (recipeCanvas != null && recipeInfoCanvas != null)
        {
            recipeCanvas.SetActive(canvas == Canvas.Recipe);
            recipeInfoCanvas.gameObject.SetActive(canvas == Canvas.Recipe);
        }

        if (questCanvas != null && questInfoCanvas != null)
        {
            questCanvas.SetActive(canvas == Canvas.Quest);
            questInfoCanvas.gameObject.SetActive(canvas == Canvas.Quest);
        }

        if (settingCanvas != null && settingInfoCanvas != null)
        {
            settingCanvas.SetActive(canvas == Canvas.Setting);
            settingInfoCanvas.gameObject.SetActive(canvas == Canvas.Setting);
        }
    }
}
