using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class WeaponUpgradeStore : MonoBehaviour
{
    private IWeaponStoreCustomer customer;
    private Dictionary<string, WeaponData> weaponDatabase;
    private Navigation[] firstSelectedButton = null;

    private bool weapon_Selection_Buttons_Initialized = false;
    private Button[] menuButtons = null;
    private VerticalLayoutGroup[] upgradePanels;

    private GameObject silverValueUI;

    private void Start()
    {
        weaponDatabase = Database._instance.GetWeaponDatabase();
    }

    public void InitWeaponUpgradeStore(GameObject _menu)
    {
        Navigation navi;

        // initialize buttons only for weapons that have been purchased
        // in the "weapon selection panel" 
        if (!weapon_Selection_Buttons_Initialized)
            SetMenuButttons(_menu);

    
        for (int i = 0; i < menuButtons.Length; i++)
        {
            var weapons = PlayerController.instance.weaponManager.Weapons;
            var button = menuButtons[i].gameObject.GetComponent<WeaponNameUpgradeButtonUI>();

            navi = menuButtons[i].navigation;
            if (!weapon_Selection_Buttons_Initialized)
            {
                firstSelectedButton[i] = navi;
                if (i == menuButtons.Length - 1)
                    weapon_Selection_Buttons_Initialized = true;
            }

            for (int j = 0; j < weapons.Length; j++)
            {
                string weaponName = weapons[j].GetComponent<Weapon>().GetWeaponName();
                if (button.WeaponName == weaponName)
                {
                    if (!weapons[j].GetComponent<Weapon>().WeaponAttributes.WeaponPurchased)
                    {
                        navi.selectOnRight = null;
                        menuButtons[i].navigation = navi;
                    }

                    else
                        menuButtons[i].navigation = firstSelectedButton[i];

                    break;
                }
            }
        }
        //ResetMenu();
        InitUpgradeSelectionButtons(_menu.transform.Find("Panel_WeaponSelectionButtons").gameObject);
    }

    // initialize upgrade buttons only for weapons that have been purchased
    // in the "weapon upgrade panel" 
    private void InitUpgradeSelectionButtons(GameObject _menu)
    {
        upgradePanels =
          _menu.transform.Find("Weapon Upgrades").GetComponentsInChildren<VerticalLayoutGroup>(true);

        var weapons = PlayerController.instance.weaponManager.Weapons;

        for (int i = 0; i <  upgradePanels.Length; i++)
        {
            if (i > weapons.Length)
                return;

            PurchaseUpgradeButtonUI[] upgradeButton = upgradePanels[i].gameObject.GetComponentsInChildren<PurchaseUpgradeButtonUI>();

            for (int j = 0; j < upgradeButton.Length; j++)
            {
                string weaponName = weapons[i].GetComponent<Weapon>().GetWeaponName();
                upgradeButton[j].WeaponName = weaponName;
                if (weapons[i].GetComponent<Weapon>().WeaponAttributes.WeaponPurchased)
                {
                    WeaponData weaponData;
                    weaponData = weaponDatabase[weaponName];

                    upgradeButton[j].GetComponent<Button>().interactable = true;
                    if(!upgradeButton[j].ButtonInit)
                        InitUpgradeButton(upgradeButton[j], weaponData, weapons[i].GetComponent<Weapon>());
                }
            }
        }
    }

    public void InitUpgradeButton(PurchaseUpgradeButtonUI _button, WeaponData _weaponData, Weapon _weapon)
    {
        PurchaseUpgradeButtonUI tmpButton = _button;

        tmpButton.InitUpgradeButton(_weapon, _weaponData);

        tmpButton.Upgrade_Button.onClick.AddListener(() => PurchaseWeaponUpgrade(_weapon.GetWeaponName(), 
            _button.UpgradeType, _button)); 
    }

    private void PurchaseWeaponUpgrade(string _weaponName, WeaponUpgradeTypes.UpgradeType upgradeType, PurchaseUpgradeButtonUI _button)
    {
        var attributes = weaponDatabase[_weaponName].AttributeDataList;
        WeaponData weaponData = weaponDatabase[_weaponName];
        Weapon weapon = null;

        int price = 0;
        var weapons = PlayerController.instance.weaponManager.Weapons;

        int weaponAttributeLvl = 0;

        //get weapon/weapon attribute/and weapon attribute level
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].GetComponent<Weapon>().GetWeaponName() == _weaponName)
            {
                weapon = weapons[i].GetComponent<Weapon>();
                weaponAttributeLvl =
                    weapons[i].GetComponent<Weapon>().WeaponAttributes.GetWeaponAttributeLevels()[upgradeType];
            }
        }

        //get price
        for (int i = 0; i < attributes.Length; i++)
        {
            if (attributes[i].UpgradeType == upgradeType)
            {
                if (weaponAttributeLvl < 3)
                {
                    if (i == attributes.Length)
                        price = attributes[i].AttributePrice;
                    else
                        price = attributes[i + 1].AttributePrice;
                    break;
                }
            }
        }

        if (customer.CanPurchaseWeapon(price) && weaponAttributeLvl <= 2)
        {
            customer.PurchaseWeaponUpgrade(_weaponName, upgradeType);
            Debug.Log("purchased upgrade: " + upgradeType + " || " 
                + " Level: " + weaponAttributeLvl 
                + " || weaponType: " + _weaponName);

            //refactor or find another method to change the button color
            //when upgrade is purchased. I think maybe applying
            //some observer patter for buttons. idk...
            _button.InitUpgradeButton(weapon, weaponData);
        }
        silverValueUI.GetComponent<DisplaySilverTotal>().SetSilverValueUI();
    }

    //customer/player is assigned at store trigger (DisplayButton.cs)
    public void AssignCustomer(IWeaponStoreCustomer _customer)
    {
        customer = _customer;
    }

    private void SetMenuButttons(GameObject _menu)
    {
        menuButtons =
           _menu.transform.Find("Panel_WeaponSelectionButtons").transform.Find("Weapon Selection Buttons").GetComponentsInChildren<Button>();
        firstSelectedButton = new Navigation[menuButtons.Length];
        silverValueUI = GameObject.Find("Text_SilverValue");
    }


    public void ResetMenu()
    {   
        for (int i = upgradePanels.Length-1; i > 0; i--)
        {
            if (i == 0)
                return;
            upgradePanels[i].gameObject.SetActive(false); 
        }
    }   
}
