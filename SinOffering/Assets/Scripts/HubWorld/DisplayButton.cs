/*
 * Display button prompts in game world when player enters a trgigger. 
 * that prompt will enable another menu to render
 * (map select, weapon pruchase/upgrade, and abiliy purchase/upgrade, etc).
 */
using System.Threading.Tasks;
using UnityEngine.EventSystems;
using UnityEngine;

public class DisplayButton : MonoBehaviour
{
    public GameObject ButtonPrompt;
    public GameObject[] NewMenu;
    public bool isLevelSelection = false;
    public bool isReturnToHUB = false;
    
    public enum MenuType
    {
        WeaponPurchaseMenu,
        WeaponUpgradeMenu,
        AbilityUpgradeMenus,
        OfferingSelectionMenu
    }
    public MenuType menuType;
    private MenuManager menuManager;

    private void Start()
    {
        if(!isLevelSelection)
            menuManager = NewMenu[0].GetComponentInParent<MenuManager>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (isLevelSelection && !isReturnToHUB)
            {
                if (!ButtonPrompt.activeInHierarchy && 
                    (!NewMenu[0].activeSelf && !NewMenu[1].activeSelf))
                    ButtonPrompt.SetActive(true);

                if (ButtonPrompt.activeInHierarchy && Input.GetButtonDown("Jump"))
                {
                    // stop player
                    PlayerController.instance.rb.velocity = Vector3.zero;
                    NewMenu[0].SetActive(true);
                    ButtonPrompt.SetActive(false);
                }
                return;
            }

            if (isReturnToHUB && !isLevelSelection)
            {
                if (!ButtonPrompt.activeInHierarchy && !NewMenu[0].activeSelf)
                    ButtonPrompt.SetActive(true);

                if (ButtonPrompt.activeInHierarchy && Input.GetButtonDown("Jump"))
                {
                    // stop player
                    PlayerController.instance.rb.velocity = Vector3.zero;
                    GetComponent<LoadScene>().LoadSceneByIndex(1);
                }
            }

            if (((int)menuType == 0 || (int)menuType == 1) &&
                !isLevelSelection)
            {
                if (!ButtonPrompt.activeInHierarchy && menuManager.EnablePrompt)
                    ButtonPrompt.SetActive(true);

                if (ButtonPrompt.activeInHierarchy && Input.GetButtonDown("Jump"))
                {
                    // assign customer
                    IWeaponStoreCustomer customer = other.GetComponent<IWeaponStoreCustomer>();
                    menuManager.GetComponent<WeaponStore>().AssignCustomer(customer);
                    menuManager.GetComponent<WeaponUpgradeStore>().AssignCustomer(customer);
                    
                    // stop player
                    PlayerController.instance.rb.velocity = Vector3.zero;
                    menuManager.EnterState((int)MenuManager.UpgradeMenu.StoreSelectionMenu);
                    ButtonPrompt.SetActive(false);
                }
            }
            if ((int)menuType == 2)
            {
                if (!ButtonPrompt.activeInHierarchy && menuManager.EnablePrompt)
                    ButtonPrompt.SetActive(true);

                if (ButtonPrompt.activeInHierarchy && Input.GetButtonDown("Jump"))
                {
                    // assign customer
                    IAttributeStoreCustomer customer = other.GetComponent<IAttributeStoreCustomer>();
                    menuManager.GetComponent<AttributeUpgradeStore>().AssignCustomer(customer);

                    // stop player
                    PlayerController.instance.rb.velocity = Vector3.zero;
                    menuManager.EnterState((int)MenuManager.UpgradeMenu.AbilityUpgradeMenu);
                    ButtonPrompt.SetActive(false);
                }
            }
            if ((int)menuType == 3)
            {
                if (!ButtonPrompt.activeInHierarchy && menuManager.EnablePrompt)
                    ButtonPrompt.SetActive(true);

                if (ButtonPrompt.activeInHierarchy && Input.GetButtonDown("Jump"))
                {
                    // stop player
                    PlayerController.instance.rb.velocity = Vector3.zero;
                    NewMenu[0].SetActive(true);
                    menuManager = NewMenu[0].GetComponent<MenuManager>();
                    menuManager.EnterState((int)MenuManager.UpgradeMenu.OfferingSelectionMenu);
                   
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
