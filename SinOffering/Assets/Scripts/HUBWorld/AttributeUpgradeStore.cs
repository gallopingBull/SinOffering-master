using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;


public class AttributeUpgradeStore : MonoBehaviour
{
    private IAttributeStoreCustomer customer;
    private Dictionary<string, AttributeData> attributeDatabase;
    public Button[] menuButtons = null;
    private GameObject silverValueUI;

    void Start()
    {
        attributeDatabase = AttributeDatabase._instance.GetAttributeDatabase();
        silverValueUI = GameObject.Find("Text_SilverValue");
    }   

    public void InitAttributeUpgradeStore(GameObject menu)
    {
        // buttons for weapon or upgrades that haven't been purchased yet
        // should be made interactable reading in data from inventory.CS ...
        List<KeyValuePair<string, AttributeData>> tmpUpgradeTypes = attributeDatabase.ToList();    
            
        if (menuButtons.Length == 0)
            SetMenuButtons(menu);

        for (int i = 0; i < menuButtons.Length - 1; i++)
        {
            // list of all buttons in Attribute Upgrade Menu
            var button = menuButtons[i].gameObject.GetComponent<AttributeUpgradeButton>();
            
            // loop through list of upgradetypes
            for (int t = 0; t < tmpUpgradeTypes.Count; t++)
            {
                // if current button upgrade is upgradeType
                if (button.UpgradeType == tmpUpgradeTypes[t].Value.UpgradeType)
                {
                    int tmpLvl = PlayerController.instance.Attributes.GetCurrentAttributeLevel(button.UpgradeType);



                    for (int j = 0; j < tmpUpgradeTypes[t].Value.AttributeDataList.Length; j++)
                    {
                        if (tmpLvl > tmpUpgradeTypes[t].Value.AttributeDataList[j].AttributeLevel)
                        {
                            menuButtons[i].interactable = false; // already purchased
                        }
                            
                        //else

                        // not purchased
                        AttributeUpgradeData attributeData = tmpUpgradeTypes[t].Value.AttributeDataList[j];
                        InitButton(button, attributeData, button.UpgradeType.ToString(), tmpLvl);

                        //next button not visible yet

                        if (menuButtons[i + 1].GetComponent<AttributeUpgradeButton>().UpgradeType != button.UpgradeType)
                            break;
                    }
                }
            }
        }
    }

    public void InitAttributeUpgradeStoreOLD(GameObject menu)
    {
        // buttons for weapon or upgrades that haven't been purchased yet
        // should be made interactable reading in data from inventory.CS ...
        List<KeyValuePair<string, AttributeData>> tmpList = attributeDatabase.ToList();

        if (menuButtons.Length == 0)
            SetMenuButtons(menu);

        for (int i = 0; i < menuButtons.Length - 1; i++)
        {
            int upgradeTypeIndex = i;

            var button = menuButtons[i].gameObject.GetComponent<AttributeUpgradeButton>();

            //menuButtons[i].interactable = false;s
            #region testing
            //int loops = i;
            //Debug.Log("loops: " + loops);
            //Debug.Log("length: " + menuButtons.Length);
            #endregion

            if (upgradeTypeIndex == 4)
                upgradeTypeIndex = 0;

            if (button.UpgradeType == tmpList[upgradeTypeIndex].Value.UpgradeType)
            {
                int tmpLvl = PlayerController.instance.Attributes.GetCurrentAttributeLevel(button.UpgradeType);

                for (int j = i; j < tmpList[upgradeTypeIndex/*i*/].Value.AttributeDataList.Length; j++)
                {
                    if (menuButtons[j + 1].GetComponent<AttributeUpgradeButton>().UpgradeType != button.UpgradeType)
                        break;

                    if (tmpLvl > tmpList[upgradeTypeIndex/*i*/].Value.AttributeDataList[j].AttributeLevel)
                    {
                        menuButtons[j].interactable = false;
                        Debug.Log("menuButtons[j].name:" + menuButtons[j].interactable);
                    }

                    AttributeUpgradeData attributeData = tmpList[upgradeTypeIndex/*i*/].Value.AttributeDataList[j];
                    InitButton(button, attributeData, button.UpgradeType.ToString(), tmpLvl);

                    Debug.Log("InitButton():" + button.name);
                    break;
                }
            }
        }
    }

    // initilaize buttons with correct weapon data 
    public void InitButton(AttributeUpgradeButton _button, AttributeUpgradeData _attributeData, string _weaponName, int curUpgradeLvl)
    {
        //_button.ItemName_Text.text = _weaponName;
        if (_attributeData.isPurchased)
        {
            _button.gameObject.GetComponent<PurchaseWeaponButtonUI>().Price_Text.text = "purchased";
            return;
        }
        _button.Price_Text.text = _attributeData.AttributePrice.ToString();
        _button.gameObject.GetComponent<Button>().onClick.AddListener(() => PurchaseUpgrade(_weaponName, curUpgradeLvl, _button.transform.gameObject.GetComponent<Button>()));
    }

    public void PurchaseUpgrade(string _upgradeType, int _upgradeLevel, Button _button)
    {
        AttributeData tmpData = attributeDatabase[_upgradeType];
        int price = tmpData.AttributeDataList[_upgradeLevel].AttributePrice; // get price

        if (customer.CanPurchaseUpgrade(price))
        {
            customer.PurchaseUpgrade(_upgradeType);
            _button.interactable = false;
            silverValueUI.GetComponent<DisplaySilverTotal>().SetSilverValueUI();
            SelectNextButton(_button);
        }
    }
   
    private void SelectNextButton(Button _button)
    {
        for (int i = 0; i < menuButtons.Length; i++)
        {
            if (_button == menuButtons[i])
            {
                EventSystem.current.SetSelectedGameObject(null);
                if (i < menuButtons.Length - 1)
                    EventSystem.current.SetSelectedGameObject(menuButtons[i + 1].gameObject);
                else
                    EventSystem.current.SetSelectedGameObject(menuButtons[0].gameObject);
            }
        }
    }
    private void SetMenuButtons(GameObject _menu)
    {
        Debug.Log("SetMenuButtons("+ _menu.name+ ")");
        menuButtons = _menu.GetComponentsInChildren<Button>();
        silverValueUI = GameObject.Find("Text_SilverValue");
    }

    // customer/player is assigned at store trigger (DisplayButton.cs)
    public void AssignCustomer(IAttributeStoreCustomer _customer)
    {
        customer = _customer;
    }
}
