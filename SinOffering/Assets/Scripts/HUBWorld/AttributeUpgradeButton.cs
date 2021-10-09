using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

[Serializable]
public class AttributeUpgradeButton : MonoBehaviour, ISelectHandler
{
    #region variables
    //[HideInInspector]
    public int UpgradeLevel = 0;

    [HideInInspector]
    public string UpgradeName;
    //[HideInInspector]
    public bool UpgradePurchased = false;
    public bool ButtonLocked = false;
    private bool buttonInit = false;

    public AttributeData data;
    public AttributeUpgradeTypes.UpgradeType UpgradeType;
    private AttributeButtonState state = AttributeButtonState.hidden;
    [SerializeField]
    private AttributeButtonColors colorState;

    [HideInInspector]
    public Button Upgrade_Button;
    public Button Parent_Button;

    #region TextMeshProGUI Components
    // UI Elements that are used for desciptions in content panel
    [HideInInspector]
    public TextMeshProUGUI UpgradeName_Text;
    [HideInInspector]
    public TextMeshProUGUI Price_Text;
    [HideInInspector]
    public TextMeshProUGUI UpgradeLevel_Text;
    //[HideInInspector]
    //public TextMeshProUGUI CurUpgradeLevel_Text;
    //[HideInInspector]
    //public TextMeshProUGUI CurUpgradeVal_Text;
    [HideInInspector]
    public TextMeshProUGUI NextUpgradeLevel_Text;
    [HideInInspector]
    public TextMeshProUGUI NextUpgradeVal_Text;
    #endregion

    #region Image Components
    public Image attributeIcon_Image;
    public Image attributeBG_Image;
    public Image[] attributeDivider_Images;
    public Image[] UpgradeLevel_Description_Images;
    public Image[] UpgradeLevel_Button_Images;
    #endregion

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

        attributeIcon_Image = transform.Find("Attribute_Icon_Image").GetComponent<Image>();
        attributeBG_Image = GetComponent<Image>();
        //attributeDivider_Image = 

        UpgradeLevel_Description_Images = new Image[5];
        UpgradeLevel_Description_Images[0] = GameObject.Find("UnlockedAttributeLevelImages").transform.Find("Image_Unlocked_Lvl_0").GetComponent<Image>();
        UpgradeLevel_Description_Images[1] = GameObject.Find("UnlockedAttributeLevelImages").transform.Find("Image_Unlocked_Lvl_1").GetComponent<Image>();
        UpgradeLevel_Description_Images[2] = GameObject.Find("UnlockedAttributeLevelImages").transform.Find("Image_Unlocked_Lvl_2").GetComponent<Image>();
        UpgradeLevel_Description_Images[3] = GameObject.Find("UnlockedAttributeLevelImages").transform.Find("Image_Unlocked_Lvl_3").GetComponent<Image>();
        UpgradeLevel_Description_Images[4] = GameObject.Find("UnlockedAttributeLevelImages").transform.Find("Image_Unlocked_Lvl_4").GetComponent<Image>();

        UpgradeLevel_Button_Images = transform.Find("AbilityUpgradeLevel_Images").GetComponentsInChildren<Image>();
       
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

    }

    public void SetButtonData(AttributeData _data, int _upgradeLevel)
    {
        //data = _data;
        //Debug.Log("InitUpgradeButton() - " + _data);

        // get player level;
        int curAttributeLevel = _upgradeLevel;



        //i want to check if dash attack is at least to level 1 or 4 to make either two buttons available or hidden
        // check button status on stems

        // top condition is only for branches (children) in the upgrade tree.
        //***** this neeeds to rafactored and debugged IMMEdiately. ****/

        if (Parent_Button != null)
        {
            //int maxLvl_dashSlam = 1;
            //int maxLvl_postDashAttack = 3;

            if (UpgradeType == AttributeUpgradeTypes.UpgradeType.dashSlam)
            {
                if (Parent_Button.GetComponent<AttributeUpgradeButton>().UpgradePurchased)
                {
                    if (UpgradeLevel == 0 && curAttributeLevel == 0)
                    {
                        Debug.Log(UpgradeType.ToString() + " is available");
                        EnterState(AttributeButtonState.available);
                    }
                    else if (UpgradeLevel == 0 && curAttributeLevel == 1)
                    {
                        Debug.Log(UpgradeType.ToString() + " is locked");
                        EnterState(AttributeButtonState.purchased);
                    }
                    else if (UpgradeLevel == 1 && curAttributeLevel == 0)
                    {
                        Debug.Log(UpgradeType.ToString() + " is locked");
                        EnterState(AttributeButtonState.locked);
                    }
                    else
                    {
                        EnterState(AttributeButtonState.available);
                    }
                }
                else
                {
                    if (UpgradeLevel == 1 && curAttributeLevel == 0)
                    {
                        Debug.Log(UpgradeType.ToString() + " is locked");
                        EnterState(AttributeButtonState.hidden);
                    }
                    if (UpgradeLevel == 0 && curAttributeLevel == 0)
                    {


                        Debug.Log(UpgradeType.ToString() + " is hidden");
                        EnterState(AttributeButtonState.hidden);
                    }
                }
            }

            if (UpgradeType == AttributeUpgradeTypes.UpgradeType.postDashAttack)
            {
                if (Parent_Button.GetComponent<AttributeUpgradeButton>().UpgradePurchased)
                {

                    if (curAttributeLevel == 0)
                    {
                        Debug.Log(UpgradeType.ToString() + " is available");
                        EnterState(AttributeButtonState.available);
                    }
                    if (curAttributeLevel == 1)
                    {
                        Debug.Log(UpgradeType.ToString() + " is locked");
                        EnterState(AttributeButtonState.purchased);
                    }
                }
                else
                {

               
                    if (UpgradeLevel == 0 && curAttributeLevel == 0)
                    {

                        Debug.Log(UpgradeType.ToString() + " is hidden");
                        EnterState(AttributeButtonState.hidden);
                    }

                }

            }
        }
        else
        {
            if (UpgradeLevel == curAttributeLevel + 1)
                EnterState(AttributeButtonState.locked);
            else if (curAttributeLevel > UpgradeLevel)
                EnterState(AttributeButtonState.purchased);
            else if (curAttributeLevel < UpgradeLevel)
                EnterState(AttributeButtonState.hidden);
            else
                EnterState(AttributeButtonState.available);
        }

        //***** this neeeds to rafactored IMMEdiately. ****/

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
        //Debug.Log(this.gameObject.name + " was selected");
        
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
        //attributeIcon_Image.color
        //attributeBG_Image.color
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
                attributeIcon_Image.color = colorState.AvailiableIconColor;
                attributeBG_Image.color = colorState.AvailiableBGColor;
                Upgrade_Button.interactable = true; // intibutton so its visible and interactive, but nothing canpurchased 
                UpgradePurchased = false;

                for (int i = 0; i < UpgradeLevel_Button_Images.Length; i++)
                    UpgradeLevel_Button_Images[i].color = Color.red;

                if (attributeDivider_Images != null)
                {
                    foreach (var divider in attributeDivider_Images)
                        divider.color = new Color(1, 1, 1, 0);
                }
            
                //Debug.Log(gameObject.name + " | " + UpgradeType.ToString() + " | " + state );
                break;

            case AttributeButtonState.purchased:
                state = AttributeButtonState.purchased;
                attributeIcon_Image.color = colorState.PurchasedIconColor;
                attributeBG_Image.color = colorState.PurchasedBGColor;
                Upgrade_Button.interactable = true;
                UpgradePurchased = true;

                for (int i = 0; i < UpgradeLevel_Button_Images.Length; i++)
                    UpgradeLevel_Button_Images[i].color = Color.red;

                if (attributeDivider_Images != null)
                {
                    foreach (var divider in attributeDivider_Images)
                        divider.color = Color.white;
                }

                //Debug.Log(gameObject.name + " | " + UpgradeType.ToString() + " | " + state);
                break;

            case AttributeButtonState.locked:
                state = AttributeButtonState.locked;
                attributeIcon_Image.color = colorState.LockedIconColor;
                attributeBG_Image.color = colorState.LockedBGColor;
                Upgrade_Button.interactable = false;
                UpgradePurchased = false;
                
                for (int i = 0; i < UpgradeLevel_Button_Images.Length; i++)
                    UpgradeLevel_Button_Images[i].color = colorState.HiddenBGColor;
                
                if (attributeDivider_Images != null)
                {
                    foreach (var divider in attributeDivider_Images)
                        divider.color = new Color(1, 1, 1, 0);
                }

                //Debug.Log(gameObject.name + " | " + UpgradeType.ToString() + " | " + state);
                break;

            case AttributeButtonState.hidden:
                state = AttributeButtonState.hidden;
                attributeIcon_Image.color = colorState.HiddenIconColor;
                attributeBG_Image.color = colorState.HiddenBGColor;
                Upgrade_Button.interactable = false;
                UpgradePurchased = false;

                for (int i = 0; i < UpgradeLevel_Button_Images.Length; i++)
                    UpgradeLevel_Button_Images[i].color = colorState.HiddenBGColor;
                
                if (attributeDivider_Images != null)
                {
                    foreach (var divider in attributeDivider_Images)
                        divider.color = new Color(1, 1, 1, 0);
                }

                //Debug.Log(gameObject.name + " | " + UpgradeType.ToString() + " | " + state);
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
        if (UpgradeLevel_Description_Images.Length > 0 && UpgradeLevel_Description_Images != null)
        {
            for (int i = 0; i < UpgradeLevel_Description_Images.Length; i++)
                UpgradeLevel_Description_Images[i].color = colorState.AvailiableBGColor;

            for (int index = 0; index < level + 1; index++)
                UpgradeLevel_Description_Images[index].color = Color.red;

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

[Serializable]
struct AttributeButtonColors
{
    //originalColor = new Color(255, 255, 255, 255/2f);
    //availiableIconColor = new Color(0, 0, 0, .5f);
    public Color AvailiableIconColor;
    public Color AvailiableBGColor;

    public Color PurchasedIconColor;
    public Color PurchasedBGColor;

    public Color LockedIconColor;
    public Color LockedBGColor;

    public Color HiddenIconColor;
    public Color HiddenBGColor;
}
