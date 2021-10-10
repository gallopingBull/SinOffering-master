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

        // switch out and get these levels from players ref to savesystem
        Dictionary<AttributeUpgradeTypes.UpgradeType, int> attributeUpgradeLevels =
            PlayerController.instance.Attributes.GetAttributeLevelData();//new Dictionary<AttributeUpgradeTypes.UpgradeType, int>();

        // list of all buttons in Attribute Upgrade Menu
        if (menuButtons.Length == 0)
            SetMenuButtons(_menu);

        var sortedList = menuButtons.OrderBy(o => (int)o.gameObject.GetComponent<AttributeUpgradeButton>().UpgradeType);

       
        for (int i = 0; i < sortedList.ToList().Count; i++)
        {
            var _button = sortedList.ToList()[i].gameObject.GetComponent<AttributeUpgradeButton>();
            int tmpLevel = attributeUpgradeLevels[_button.UpgradeType];

            if(i > 0 && i < sortedList.ToList().Count -1 )
            {
                if (_button.UpgradeType ==
                sortedList.ToList()[i + 1].gameObject.GetComponent<AttributeUpgradeButton>().UpgradeType)
                {
                    _button.Child_Button = sortedList.ToList()[i + 1];
                }
                
            }

            Debug.Log("UpgradeType: " + _button.UpgradeType.ToString() + " || " + "Upgrade Level: " + tmpLevel);  
            AttributeUpgradeData attributeData = _upgradeTypes[(int)_button.UpgradeType].Value.AttributeDataList[_button.UpgradeLevel];
            InitButton(_button, attributeData, _button.UpgradeType.ToString(), tmpLevel);
            
        }
    }

    // initilaize buttons with correct weapon data 
    public void InitButton(AttributeUpgradeButton _button, AttributeUpgradeData _attributeData, string _upgradeName, int curUpgradeLvl)
    {
        _button.SetButtonData(attributeDatabase[_button.UpgradeType.ToString()], curUpgradeLvl);
        if (!_button.UpgradePurchased)
        {
            _button.gameObject.GetComponent<Button>().onClick.AddListener(() =>
        PurchaseUpgrade(_upgradeName, curUpgradeLvl, _button.transform.gameObject.GetComponent<Button>()));
        }
        
    }


    public void PurchaseUpgrade(string _upgradeType, int _upgradeLevel, Button _button)
    {
        AttributeData tmpData = attributeDatabase[_upgradeType];
        int price = tmpData.AttributeDataList[_upgradeLevel].AttributePrice; // get price

        if (customer.CanPurchaseUpgrade(price))
        {
            customer.PurchaseUpgrade(_upgradeType);
            _button.interactable = false;


            _button.GetComponent<AttributeUpgradeButton>().PurchaseUpgrade(); // changes button faith
            silverValueUI.GetComponent<DisplaySilverTotal>().SetSilverValueUI(); // change to faith
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
