using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class AttributeUpgradeButton : MonoBehaviour, ISelectHandler
{
    #region variables

    public AttributeData data;
    //[HideInInspector]
    public int UpgradeLevel = 0;

    AttributeButtonState state = AttributeButtonState.hidden;

    [HideInInspector]
    public string UpgradeName;

    public bool ButtonLocked = false;

    public AttributeUpgradeTypes.UpgradeType UpgradeType;

    // UI Elements that are used for desciptions in content panel
    [HideInInspector]
    public TextMeshProUGUI UpgradeName_Text;

    [HideInInspector]
    public TextMeshProUGUI Price_Text;
    [HideInInspector]
    public TextMeshProUGUI UpgradeLevel_Text;

    [HideInInspector]
    public TextMeshProUGUI CurUpgradeLevel_Text;
    [HideInInspector]
    public TextMeshProUGUI CurUpgradeVal_Text;
    [HideInInspector]
    public TextMeshProUGUI NextUpgradeLevel_Text;

    [HideInInspector]
    public TextMeshProUGUI NextUpgradeVal_Text;

    //[HideInInspector]
    public bool UpgradePurchased = false;


    //[HideInInspector]
    public Image[] UpgradeLevelImages;
    [HideInInspector]
    public Button Upgrade_Button;

    public Color originalColor;

    private bool buttonInit = false;

    public bool ButtonInit { get => buttonInit; private set => buttonInit = value; }
    #endregion
    #region functions
    private void Awake()
    {
        var database = AttributeDatabase._instance.GetAttributeDatabase();
        data = database[UpgradeType.ToString()];

        Upgrade_Button = GetComponent<Button>();
        UpgradeName = UpgradeType.ToString();
        UpgradeName_Text =
            GameObject.Find("Text_AbilityName").GetComponent<TextMeshProUGUI>();
        UpgradeName_Text.text = UpgradeType.ToString();

        UpgradeLevelImages = new Image[5];
        UpgradeLevelImages[0] = GameObject.Find("UnlockedAttributeLevelImages").transform.Find("Image_Unlocked_Lvl_0").GetComponent<Image>();
        UpgradeLevelImages[1] = GameObject.Find("UnlockedAttributeLevelImages").transform.Find("Image_Unlocked_Lvl_1").GetComponent<Image>();
        UpgradeLevelImages[2] = GameObject.Find("UnlockedAttributeLevelImages").transform.Find("Image_Unlocked_Lvl_2").GetComponent<Image>();
        UpgradeLevelImages[3] = GameObject.Find("UnlockedAttributeLevelImages").transform.Find("Image_Unlocked_Lvl_3").GetComponent<Image>();
        UpgradeLevelImages[4] = GameObject.Find("UnlockedAttributeLevelImages").transform.Find("Image_Unlocked_Lvl_4").GetComponent<Image>();

        Price_Text =
              GameObject.Find("Panel_AbilityPrice").transform.Find("Text_SilverValue").GetComponent<TextMeshProUGUI>();

        #region testing/old code
        //CurUpgradeLevel_Text =    
        //GameObject.Find("Text_AbilityCurLevel").GetComponent<TextMeshProUGUI>();

        //CurUpgradeVal_Text =
        //transform.Find("Current Level UI Objects").transform.Find("Text_UpgradeValue").GetComponent<TextMeshProUGUI>();
        #endregion

        NextUpgradeLevel_Text =
            GameObject.Find("Panel_Content").transform.Find("Panel").transform.Find("Text_AbilityName").transform.Find("Text_AbilityCurLevel").GetComponentInChildren<TextMeshProUGUI>();

        NextUpgradeVal_Text =
            GameObject.Find("Panel_Content").transform.Find("Text_AbilityDescription").GetComponent<TextMeshProUGUI>();

        //originalColor = new Color(255, 255, 255, 255/2f);
        originalColor = new Color(0, 0, 0, .5f);
    }

    public void SetButtonData(AttributeData _data, int _upgradeLevel)
    {
        Awake();
        //AttributeData tmpData = _data;

        //data = _data;
        //Debug.Log("InitUpgradeButton() - " + _data);

        // get player level;
        //int tmpLevel = _data.atttributeLevel;
        int tmpLevel = _upgradeLevel;

        if (tmpLevel >= UpgradeLevel)
        {
            EnterState(AttributeButtonState.purchased);
        }
        else if (tmpLevel < UpgradeLevel)
        {
            EnterState(AttributeButtonState.locked);
        }



        UpgradeName_Text.text = UpgradeType.ToString();

        for (int i = 0; i < data.AttributeDataList.Length; i++)
        {
            if (UpgradeType == data.UpgradeType && 
                data.AttributeDataList[i].AttributeLevel == UpgradeLevel)
            {
                if (UpgradeLevel < data.AttributeDataList.Length)
                {
                    Price_Text.text = data.AttributeDataList[i].AttributePrice.ToString();

                    NextUpgradeLevel_Text.text = (UpgradeLevel).ToString();
                    NextUpgradeVal_Text.text = data.AttributeDataList[i].AttributeValue.ToString();
                }
                SetImageColors(UpgradeLevel);
            }
        }
        buttonInit = true;
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log(this.gameObject.name + " was selected");

        UpgradeName_Text.text = UpgradeType.ToString();
       
        Price_Text.text = data.AttributeDataList[UpgradeLevel].AttributePrice.ToString();
        NextUpgradeLevel_Text.text = data.AttributeDataList[UpgradeLevel].AttributeLevel.ToString();
        NextUpgradeVal_Text.text = data.AttributeDataList[UpgradeLevel].UpgradeDescription;
        SetImageColors(UpgradeLevel);
    }

    public void SetUpgradeLevel(int _upgradeLevel)
    {
        if (UpgradeType == AttributeUpgradeTypes.UpgradeType.dashAttack ||
            UpgradeType == AttributeUpgradeTypes.UpgradeType.dashSlam ||
            UpgradeType == AttributeUpgradeTypes.UpgradeType.postDashAttack ||
            UpgradeType == AttributeUpgradeTypes.UpgradeType.strength ||
            UpgradeType == AttributeUpgradeTypes.UpgradeType.evade)
            return;
        
        UpgradeLevel = _upgradeLevel;
    }

    void HideUpgradeButton()
    {

    }

    void DisplayUpgradeButton()
    {

    }


    private void EnterState(AttributeButtonState _state)
    {
        ExitState(state);
        switch (_state)
        {
            case AttributeButtonState.available:
                state = AttributeButtonState.available;
                Upgrade_Button.interactable = true; // intibutton so its visible and interactive, but nothing canpurchased 
                // intibutton so its visible and interactive, but nothing canpurchased
                break;

            case AttributeButtonState.purchased:
                state = AttributeButtonState.purchased;
                Upgrade_Button.interactable = true;
                UpgradePurchased = true;
                break;

            case AttributeButtonState.locked:
                state = AttributeButtonState.locked;
                Upgrade_Button.interactable = false;
                UpgradePurchased = false;
                break;

            case AttributeButtonState.hidden:
                state = AttributeButtonState.hidden;
                Upgrade_Button.interactable = false;
                UpgradePurchased = false;
                break;

            default:
                break;
        }
    }

    private void ExitState(AttributeButtonState _state)
    {
        switch (_state)
        {
            case AttributeButtonState.available:
                break;
            case AttributeButtonState.purchased:
                break;
            case AttributeButtonState.locked:
                break;
            case AttributeButtonState.hidden:
                break;

            default:
                break;
        }
    }

    public void SetImageColors(int level)
    {
        if (UpgradeLevelImages.Length > 0 && UpgradeLevelImages != null)
        {
            for (int i = 0; i < UpgradeLevelImages.Length; i++)
                UpgradeLevelImages[i].color = originalColor;

            for (int index = 0; index < level + 1; index++)
                UpgradeLevelImages[index].color = Color.red;

            //for (int i = UpgradeLevelImages.Length - 1; i > level; i--)
            //UpgradeLevelImages[i].color = Color.black;
        }
    }

    #endregion

}


public enum AttributeButtonState
{
    available,
    purchased,
    locked,
    hidden
}
