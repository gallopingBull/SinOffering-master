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

        if (menuButtons.Length == 0)
            SetMenuButtons(menu);


        for (int i = 0; i < menuButtons.Length; i++)
        {
            var weapons = PlayerController.instance.weaponManager.Weapons;
            var button = menuButtons[i].gameObject.GetComponent<PurchaseWeaponButtonUI>();

            for (int j = 0; j < weapons.Length; j++)
            {
                string weaponName = weapons[j].GetComponent<Weapon>().GetWeaponName();
                if (button.ItemName == weaponName)
                {
                    if (weapons[j].GetComponent<Weapon>().WeaponAttributes.WeaponPurchased)
                        menuButtons[i].interactable = false;

                    //AttributeUpgradeData attributeData = attributeDatabase[weaponName].AttributeDataList;
                    //InitButton(button, attributeData, weaponName,
                        //weapons[j].GetComponent<Weapon>().WeaponAttributes.WeaponPurchased);

                    break;
                }
            }
        }
    }

    // initilaize buttons with correct weapon data 
    public void InitButton(PurchaseWeaponButtonUI _button, AttributeUpgradeData _attributeData, string _weaponName, bool purchased)
    {
        _button.ItemName_Text.text = _weaponName;
        if (purchased)
        {
            _button.gameObject.GetComponent<PurchaseWeaponButtonUI>().Price_Text.text = "purchased";
            return;
        }
        _button.Price_Text.text = _attributeData.AttributePrice.ToString();
        _button.gameObject.GetComponent<Button>().onClick.AddListener(() => PurchaseUpgrade(_weaponName, _button.transform.gameObject.GetComponent<Button>()));
    }

    public void PurchaseUpgrade(string _weaponName, Button _button)
    {
        List<KeyValuePair<string, AttributeData>> tmpList = attributeDatabase.ToList();
        int price = 0; // get price

        if (customer.CanPurchaseUpgrade(price))
        {
            customer.PurchaseUpgrade(_weaponName);
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
        menuButtons = _menu.GetComponentsInChildren<Button>();
        silverValueUI = GameObject.Find("Text_SilverValue");
    }

    // customer/player is assigned at store trigger (DisplayButton.cs)
    public void AssignCustomer(IAttributeStoreCustomer _customer)
    {
        customer = _customer;
    }
}
