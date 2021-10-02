using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems; 
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class AttributeUpgradeButton : MonoBehaviour
{
    #region variables

    [HideInInspector]
    public string UpgradeName;

    public AttributeUpgradeTypes.UpgradeType UpgradeType;

    // UI Elements
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
    public Image[] UpgradeLevelImages;
    [HideInInspector]
    public Button Upgrade_Button;

    private bool buttonInit = false;

    public bool ButtonInit { get => buttonInit; private set => buttonInit = value; }
    #endregion

    #region functions
    private void Awake()
    {
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

        //CurUpgradeLevel_Text =    
        //GameObject.Find("Text_AbilityCurLevel").GetComponent<TextMeshProUGUI>();

        //CurUpgradeVal_Text =
        //transform.Find("Current Level UI Objects").transform.Find("Text_UpgradeValue").GetComponent<TextMeshProUGUI>();


        NextUpgradeLevel_Text =
            GameObject.Find("Panel_Content").transform.Find("Text_AbilityCurLevel").GetComponent<TextMeshProUGUI>();

        NextUpgradeVal_Text =
            GameObject.Find("Panel_Content").transform.Find("Text_AbilityCurLevel").GetComponent<TextMeshProUGUI>();

    }

    public void InitUpgradeButton(AttributeData _data)
    {
        Awake();
        AttributeData tmpData = _data;
        // get player level;
        int tmpLevel = PlayerController.instance.Attributes.GetCurrentAttributeLevel(UpgradeType);

        for (int i = 0; i < tmpData.AttributeDataList.Length; i++)
        {
            if (UpgradeType == tmpData.UpgradeType &&
                tmpData.AttributeDataList[i].AttributeLevel == tmpLevel)
            {
                CurUpgradeVal_Text.text = tmpData.AttributeDataList[i].AttributeValue.ToString();
                CurUpgradeLevel_Text.text = tmpLevel.ToString();
                if (tmpLevel < 3)
                {
                    Price_Text.text = tmpData.AttributeDataList[i + 1].AttributePrice.ToString();

                    NextUpgradeLevel_Text.text = (tmpLevel + 1).ToString();
                    NextUpgradeVal_Text.text = tmpData.AttributeDataList[i + 1].AttributeValue.ToString();
                }
                else
                {   
                    NextUpgradeLevel_Text.text = "--";
                    NextUpgradeVal_Text.text = "--";
                    Price_Text.text = "--";
                }
                SetImageColors(tmpLevel);
            }
        }
        buttonInit = true;
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log(GetComponent<Button>().name + " was selected");

    }
    public void SetImageColors(int level)
    {
        for (int i = 0; i < level; i++)
            UpgradeLevelImages[i].color = Color.red;
    }

    #endregion
}
