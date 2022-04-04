using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// class for store that allows player to purchase weapon upgrades.
/// </summary>

public class WeaponUpgradeStore : MonoBehaviour
{
    private IWeaponStoreCustomer _customer;
    private Dictionary<string, WeaponData> _weaponDatabase;
    private Navigation[] _firstSelectedButton = null;

    private bool weapon_Selection_Buttons_Initialized = false;
    private Button[] _menuButtons = null;
    private VerticalLayoutGroup[] _upgradePanels;

    private HUDManager _hm;
    private GameObject _silverValueUI;

    private void Start()
    {
        _weaponDatabase = WeaponDatabase._instance.GetWeaponDatabase();
    }

    public void InitWeaponUpgradeStore(GameObject menu)
    {
        Navigation navi;

        // initialize buttons only for weapons that have been purchased
        // in the "weapon selection panel" 
        if (!weapon_Selection_Buttons_Initialized)
            SetMenuButttons(menu);

    
        for (int i = 0; i < _menuButtons.Length; i++)
        {
            var weapons = PlayerController.instance.weaponManager.Weapons;  
            var button = _menuButtons[i].gameObject.GetComponent<WeaponNameUpgradeButtonUI>();

            navi = _menuButtons[i].navigation;
            if (!weapon_Selection_Buttons_Initialized)
            {
                _firstSelectedButton[i] = navi;
                if (i == _menuButtons.Length - 1)
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
                        _menuButtons[i].navigation = navi;
                    }

                    else
                        _menuButtons[i].navigation = _firstSelectedButton[i];

                    break;
                }
            }
        }
        //ResetMenu();
        InitUpgradeSelectionButtons(menu.transform.Find("Panel_WeaponSelectionButtons").gameObject);
    }

    // initialize upgrade buttons only for weapons that
    // have been purchased in the "weapon upgrade panel" 
    private void InitUpgradeSelectionButtons(GameObject menu)
    {
        _upgradePanels =
          menu.transform.Find("Weapon Upgrades").GetComponentsInChildren<VerticalLayoutGroup>(true);

        var weapons = PlayerController.instance.weaponManager.Weapons;

        for (int i = 0; i <  _upgradePanels.Length; i++)
        {
            if (i > weapons.Length)
                return;

            PurchaseUpgradeButtonUI[] upgradeButton = _upgradePanels[i].gameObject.GetComponentsInChildren<PurchaseUpgradeButtonUI>();

            for (int j = 0; j < upgradeButton.Length; j++)
            {
                string weaponName = weapons[i].GetComponent<Weapon>().GetWeaponName();
                upgradeButton[j].WeaponName = weaponName;
                if (weapons[i].GetComponent<Weapon>().WeaponAttributes.WeaponPurchased)
                {
                    WeaponData weaponData;
                    weaponData = _weaponDatabase[weaponName] as WeaponData;

                    upgradeButton[j].GetComponent<Button>().interactable = true;
                    if(!upgradeButton[j].ButtonInit)
                        InitUpgradeButton(upgradeButton[j], weaponData, weapons[i].GetComponent<Weapon>());
                }
            }
        }
    }

    public void InitUpgradeButton(PurchaseUpgradeButtonUI button, ScriptableObject data, Weapon weapon)
    {
        PurchaseUpgradeButtonUI tmpButton = button;

        tmpButton.InitUpgradeButton(weapon, data);

        tmpButton.Upgrade_Button.onClick.AddListener(() => PurchaseWeaponUpgrade(weapon.GetWeaponName(), 
            button.UpgradeType, button)); 
    }

    private void PurchaseWeaponUpgrade(string weaponName, WeaponUpgradeTypes.UpgradeType upgradeType, PurchaseUpgradeButtonUI button)
    {
        WeaponData weaponData = _weaponDatabase[weaponName] as WeaponData;
        var attributes = weaponData.AttributeDataList;
        Weapon weapon = null;

        int price = 0;
        var weapons = PlayerController.instance.weaponManager.Weapons;

        int weaponAttributeLvl = 0;

        // get weapon/weapon attribute/and weapon attribute level
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].GetComponent<Weapon>().GetWeaponName() == weaponName)
            {
                weapon = weapons[i].GetComponent<Weapon>();
                weaponAttributeLvl =
                    weapons[i].GetComponent<Weapon>().WeaponAttributes.GetWeaponAttributeLevels()[upgradeType];
            }
        }

        // get price
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

        if (_customer.CanPurchaseWeapon(price) && weaponAttributeLvl <= 2)
        {
            _customer.PurchaseWeaponUpgrade(weaponName, upgradeType);
            Debug.Log("purchased upgrade: " + upgradeType + " || " 
                + " Level: " + weaponAttributeLvl 
                + " || weaponType: " + weaponName);

            // refactor or find another method to change the button color
            // when upgrade is purchased. I think maybe applying
            // some observer patter for buttons. idk...
            button.InitUpgradeButton(weapon, weaponData);
        }
        _silverValueUI.GetComponent<DisplaySilverTotal>().SetSilverValueUI();
        _hm.SetUIObjectValues();
    }

    // customer/player is assigned at store trigger (DisplayButton.cs)
    public void AssignCustomer(IWeaponStoreCustomer customer) => _customer = customer;

    private void SetMenuButttons(GameObject menu)
    {
        _menuButtons =
           menu.transform.Find("Panel_WeaponSelectionButtons").transform.Find("Weapon Selection Buttons").GetComponentsInChildren<Button>();
        _firstSelectedButton = new Navigation[_menuButtons.Length];
        _silverValueUI = GameObject.Find("Text_SilverValue");
        _hm = HUDManager._instance; 
    }

    public void ResetMenu()
    {   
        for (int i = _upgradePanels.Length-1; i > 0; i--)
        {
            if (i == 0)
                return;
            _upgradePanels[i].gameObject.SetActive(false); 
        }
    }   
}
