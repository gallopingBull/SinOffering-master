/*
 * Display button prompts in game world when player enters a trgigger. 
 * that prompt will enable another menu to render
 * (map select, weapon pruchase/upgrade, and abiliy purchase/upgrade, etc).
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayButton : MonoBehaviour
{
    public GameObject ButtonPrompt;
    public GameObject[] NewMenu;
    public bool isLevelSelection = false;
    public enum MenuType
    {
        WeaponPurchaseMenu,
        WeaponUpgradeMenu,
        AbilityUpgradeMenus
    }

    public MenuType menuType;


    private MenuManager weaponMenuManager;


    private void Start()
    {
        if(!isLevelSelection)
            weaponMenuManager = NewMenu[0].GetComponentInParent<MenuManager>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (isLevelSelection)
            {
                if (!ButtonPrompt.activeInHierarchy)
                    ButtonPrompt.SetActive(true);

                if (ButtonPrompt.activeInHierarchy && Input.GetButtonDown("Jump"))
                {
                    //stop player
                    PlayerController.instance.rb.velocity = Vector3.zero;
                    NewMenu[0].SetActive(true);
                    ButtonPrompt.SetActive(false);
                }
            }
            if(((int)menuType == 0 || (int)menuType == 1) &&
                !isLevelSelection)
            {
                if (!ButtonPrompt.activeInHierarchy && weaponMenuManager.EnablePrompt)
                    ButtonPrompt.SetActive(true);

                if (ButtonPrompt.activeInHierarchy && Input.GetButtonDown("Jump"))
                {
                    //assign customer
                    IWeaponStoreCustomer customer = other.GetComponent<IWeaponStoreCustomer>();
                    weaponMenuManager.GetComponent<WeaponStore>().AssignCustomer(customer);
                    weaponMenuManager.GetComponent<WeaponUpgradeStore>().AssignCustomer(customer);
                    
                    
                    //stop player
                    PlayerController.instance.rb.velocity = Vector3.zero;
                    weaponMenuManager.EnterState((int)MenuManager.UpgradeMenu.StoreSelectionMenu);
                    ButtonPrompt.SetActive(false);
                }
            }
            if ((int)menuType == 2)
            {
                if (!ButtonPrompt.activeInHierarchy && weaponMenuManager.EnablePrompt)
                    ButtonPrompt.SetActive(true);

                if (ButtonPrompt.activeInHierarchy && Input.GetButtonDown("Jump"))
                {
                    //assign customer
                    //IWeaponStoreCustomer customer = other.GetComponent<IWeaponStoreCustomer>();
                    //weaponMenuManager.GetComponent<WeaponStore>().AssignCustomer(customer);

                    //stop player
                    PlayerController.instance.rb.velocity = Vector3.zero;
                    weaponMenuManager.EnterState((int)MenuManager.UpgradeMenu.AbilityUpgradeMenu);
                    ButtonPrompt.SetActive(false);
                }
            }
        }  
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            ButtonPrompt.SetActive(false);
    }
}
