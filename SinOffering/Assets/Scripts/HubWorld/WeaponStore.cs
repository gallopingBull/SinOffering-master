using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class WeaponStore : MonoBehaviour
{
    private IWeaponStoreCustomer customer;
    private Dictionary<string, WeaponData> weaponDatabase;
    public Button[] menuButtons = null;
    private GameObject silverValueUI;

    private void Start()    
    {
        weaponDatabase = Database._instance.GetWeaponDatabase();
        silverValueUI = GameObject.Find("Text_SilverValue");
    }

    public void InitWeaponStore(GameObject menu)
    {
        //buttons for weapon or upgrades that haven't been purchased yet
        //should be made interactable reading in data from inventory.CS
        //...

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
                    
                    WeaponData weaponData; 
                    weaponData = weaponDatabase[weaponName];
                    InitButton(button, weaponData, weaponName,
                        weapons[j].GetComponent<Weapon>().WeaponAttributes.WeaponPurchased);

                    break;
                }
            }
        }
    }

    //initilaize buttons' with weapon   data 
    public void InitButton(PurchaseWeaponButtonUI _button, WeaponData _weaponData, string _weaponName, bool purchased)
    {
        _button.ItemName_Text.text = _weaponName;
        if (purchased)
        {
            _button.gameObject.GetComponent<PurchaseWeaponButtonUI>().Price_Text.text = "purchased";
            return;
        }
        _button.Price_Text.text = _weaponData.WeaponPrice.ToString();
        _button.gameObject.GetComponent<Button>().onClick.AddListener(() => PurchaseWeapon(_weaponName, _button.transform.gameObject.GetComponent<Button>()));
    }

    public void PurchaseWeapon(string _weaponName, Button _button)
    {
        int price = weaponDatabase[_weaponName].WeaponPrice;

        if (customer.CanPurchaseWeapon(price))
        {
            customer.PurchaseWeapon(_weaponName);
            _button.interactable = false;
            silverValueUI.GetComponent<DisplaySilverTotal>().SetSilverValueUI();
            SelectNextButton(_button);
        }
    }

    //customer/player is assigned at store trigger (DisplayButton.cs)
    public void AssignCustomer(IWeaponStoreCustomer _customer)
    {
        customer = _customer;
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
}
