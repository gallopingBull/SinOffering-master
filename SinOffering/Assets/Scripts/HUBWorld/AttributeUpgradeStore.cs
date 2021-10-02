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

    public void InitAttributeUpgradeStore(GameObject _menu)
    {
        // buttons for weapon or upgrades that haven't been purchased yet
        // should be made interactable reading in data from inventory.CS ...
        List<KeyValuePair<string, AttributeData>> _upgradeTypes = attributeDatabase.ToList();    

        if (menuButtons.Length == 0)
            SetMenuButtons(_menu);

        for (int i = 0; i < menuButtons.Length - 1; i++)
        {
            // list of all buttons in Attribute Upgrade Menu
            var _button = menuButtons[i].gameObject.GetComponent<AttributeUpgradeButton>();
            int tmpLvl = PlayerController.instance.Attributes.GetCurrentAttributeLevel(_button.UpgradeType) + 1;
            Debug.Log("UpgradeType: " + _button.UpgradeType.ToString() + " || " + "Upgrade Level: " + tmpLvl);

            // loop through list of upgradetypes
            for (int typeIndex = 0; typeIndex < _upgradeTypes.Count; typeIndex++)
            {
                //Debug.Log("UpgradeType: "+ _button.UpgradeType.ToString());
              
                // if current button upgrade is not datanase upgradeType
                if (_button.UpgradeType == _upgradeTypes[typeIndex].Value.UpgradeType)
                {
                    for (int upgradeDataIndex = 0; upgradeDataIndex < _upgradeTypes[typeIndex].Value.AttributeDataList.Length; upgradeDataIndex++)
                    {
                        // checks for first element on top of every 'upgrade tree' column
                        if (_button.gameObject.name == "Ability_Name_Level_Button")
                        {
                            if (_upgradeTypes[typeIndex].Value.AttributeDataList[upgradeDataIndex].AttributeLevel == 1)
                                _button.GetComponent<Button>().interactable = true;
                        }
                        // checks at every button that isn't on the first/top element in upgrade tree column
                        else
                        {
                            // button already purchased
                            if (tmpLvl > _upgradeTypes[typeIndex].Value.AttributeDataList[upgradeDataIndex].AttributeLevel)
                                _button.GetComponent<Button>().interactable = true; // intibutton so its visible and interactive, but nothing canpurchased
                            
                            if (tmpLvl < _upgradeTypes[typeIndex].Value.AttributeDataList[upgradeDataIndex].AttributeLevel)
                            {
                                //if (menuButtons[i + 1].GetComponent<AttributeUpgradeButton>().UpgradeType != _button.UpgradeType)
                                //{
                                    // show image
                                    //menuButtons[i +1].interactable = false;
                                //}
                                //hide image
                                _button.GetComponent<Button>().interactable = false;
                                //menuButtons[i +1].interactable = false;
                                break;
                            }
                        }
                        
                        AttributeUpgradeData attributeData = _upgradeTypes[typeIndex].Value.AttributeDataList[upgradeDataIndex];
                        InitButton(_button, attributeData, _button.UpgradeType.ToString(), tmpLvl);

                        if (menuButtons[i + 1].GetComponent<AttributeUpgradeButton>().UpgradeType != _button.UpgradeType)
                            break;
                    }
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
            _button.gameObject.GetComponent<PurchaseUpgradeButtonUI>(   ).Price_Text.text = "purchased";
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
        //Debug.Log("SetMenuButtons("+ _menu.name+ ")");
        menuButtons = _menu.GetComponentsInChildren<Button>();
        silverValueUI = GameObject.Find("Text_SilverValue");
    }

    // customer/player is assigned at store trigger (DisplayButton.cs)
    public void AssignCustomer(IAttributeStoreCustomer _customer)
    {
        customer = _customer;
    }
}
