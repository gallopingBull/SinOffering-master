using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PurchaseUpgradeButtonUI : MonoBehaviour
{
    #region variables
    [HideInInspector]
    public string WeaponName;
    [HideInInspector]
    public string UpgradeName;
    
    public WeaponUpgradeTypes.UpgradeType UpgradeType;
    
    // UI Elements
    [HideInInspector]
    public TextMeshProUGUI UpgradeName_Text;

    [HideInInspector]
    public TextMeshProUGUI WeapomName_Text;

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
            transform.Find("Text_UpgradeTitle").GetComponent<TextMeshProUGUI>();
        UpgradeName_Text.text = UpgradeName;

        UpgradeLevelImages = new Image[3];
        UpgradeLevelImages[0] = transform.Find("Panel_Upgrade Levels").transform.Find("Upgrade Level Image 0").GetComponent<Image>();
        UpgradeLevelImages[1] = transform.Find("Panel_Upgrade Levels").transform.Find("Upgrade Level Image 1").GetComponent<Image>();
        UpgradeLevelImages[2] = transform.Find("Panel_Upgrade Levels").transform.Find("Upgrade Level Image 2").GetComponent<Image>();

        Price_Text =
            transform.Find("Panel_Price").transform.Find("Text_UpgradePrice").GetComponent<TextMeshProUGUI>();

        CurUpgradeLevel_Text =
            transform.Find("Current Level UI Objects").transform.Find("Text_currentLevel").GetComponent<TextMeshProUGUI>();

        CurUpgradeVal_Text =
         transform.Find("Current Level UI Objects").transform.Find("Text_UpgradeValue").GetComponent<TextMeshProUGUI>();


        NextUpgradeLevel_Text =
            transform.Find("Next Level UI Upgrades").transform.Find("Text_currentLevel").GetComponent<TextMeshProUGUI>();

        NextUpgradeVal_Text = 
            transform.Find("Next Level UI Upgrades").transform.Find("Text_UpgradeValue").GetComponent<TextMeshProUGUI>();
    }

    public void InitUpgradeButton(Weapon weapon, ScriptableObject _data)
    {
        Awake();
        WeaponData tmpData = _data as WeaponData;
        int tmpLevel = weapon.WeaponAttributes.GetWeaponAttributeLevels()[UpgradeType];

        for (int i = 0; i <tmpData.AttributeDataList.Length; i++)
        {
            if (UpgradeType == tmpData.AttributeDataList[i].UpgradeType &&
                tmpData.AttributeDataList[i].AttributeLevel == tmpLevel)
            {
                CurUpgradeVal_Text.text = tmpData.AttributeDataList[i].AttributeValue.ToString();
                CurUpgradeLevel_Text.text = tmpLevel.ToString();
                if (tmpLevel < 3)
                {
                    Price_Text.text = tmpData.AttributeDataList[i + 1].AttributePrice.ToString();

                    NextUpgradeLevel_Text.text = (tmpLevel + 1).ToString();
                    NextUpgradeVal_Text.text = tmpData.AttributeDataList[i+1].AttributeValue.ToString();
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

    public void SetImageColors(int level)
    {
        for (int i = 0; i < level; i++)
            UpgradeLevelImages[i].color = Color.red;
    }

    #endregion
}