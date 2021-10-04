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


    [HideInInspector]
    public string UpgradeName;

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
    public Image[] UpgradeLevelImages;
    [HideInInspector]
    public Button Upgrade_Button;

    private Color originalColor;

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

        originalColor = UpgradeLevelImages[0].color;
    }

    public void SetButtonData(AttributeData _data)
    {
        Awake();
        //AttributeData tmpData = _data;
     
        data = _data;
        //Debug.Log("InitUpgradeButton() - " + _data);
        
        // get player level;
        //int tmpLevel = PlayerController.instance.Attributes.GetCurrentAttributeLevel(UpgradeType);

        UpgradeName_Text.text = UpgradeType.ToString();

        for (int i = 0; i < data.AttributeDataList.Length; i++)
        {
            if (UpgradeType == data.UpgradeType &&
                data.AttributeDataList[i].AttributeLevel == UpgradeLevel)
            {
                //CurUpgradeVal_Text.text = data.AttributeDataList[i].AttributeValue.ToString();
                //CurUpgradeLevel_Text.text = tmpLevel.ToString();
                if (UpgradeLevel < 3)
                {
                    Price_Text.text = data.AttributeDataList[i + 1].AttributePrice.ToString();

                    NextUpgradeLevel_Text.text = (UpgradeLevel + 1).ToString();
                    NextUpgradeVal_Text.text = data.AttributeDataList[i + 1].AttributeValue.ToString();
                }
                else
                {   
                    NextUpgradeLevel_Text.text = "--";
                    NextUpgradeVal_Text.text = "--";
                    Price_Text.text = "--";
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

    void HideUpgradeButton()
    {

    }

    void DisplayUpgradeButton()
    {

    }   


    public void SetImageColors(int level)
    {
       
        for (int i = 0; i < UpgradeLevelImages.Length; i++)
            UpgradeLevelImages[i].color = originalColor;
        for (int i = 0; i < level + 1; i++)
            UpgradeLevelImages[i].color = Color.red;
    }

    #endregion
}
