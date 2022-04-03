using System.Collections.Generic;
using System.Collections;
using Cinemachine;
using UnityEngine;

// weapon/ability menu manager
public class MenuManager : MonoBehaviour
{
    #region variables
    [HideInInspector]
    public bool EnablePrompt = true;
    private bool _inMenu = false;
    [HideInInspector]
    public enum UpgradeMenu
    {
        StoreSelectionMenu,
        WeaponPurchaseMenu,
        WeaponUpgradeMenu,
        AbilityUpgradeMenu,
        OfferingSelectionMenu,
    }

    [HideInInspector]
    public UpgradeMenu currentMenu;
    public GameObject[] menus;
    public CinemachineVirtualCamera[] menuCameras;

    [SerializeField] float Delay = 1f;
    private bool closeMenu = false;

    private Dictionary<string, WeaponData> weaponDatabase;
    private WeaponData weaponData;
    #endregion

    #region functions
    private void Start()
    {
        weaponDatabase = WeaponDatabase._instance.GetWeaponDatabase();
    }
    private void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            // press back button while in menu this gets 
            // executed twice with weapon upgrade store.
            if ((int)currentMenu == 0 && menus[0].activeInHierarchy)
                closeMenu = true;

            ExitState((int)currentMenu);
        }
    }
    public void EnterState(int state)
    {
        PlayerController.instance.DisableInput();
        currentMenu = (UpgradeMenu)state;
        StartCoroutine("Enter_State");
    }
    private IEnumerator Enter_State()
    {
        switch (currentMenu)
        {
            case UpgradeMenu.StoreSelectionMenu:
                if (!_inMenu)
                {
                    _inMenu = true;
                    UIEvents.OnStoreMenuOpened?.Invoke();
                }
                EnablePrompt = false;
                SwitchCamera((int)currentMenu);
                yield return new WaitForSeconds(Delay/2);
                DisplayMenu((int)currentMenu);

                break;

            case UpgradeMenu.WeaponPurchaseMenu:
                SwitchCamera((int)currentMenu);
                yield return new WaitForSeconds(Delay);
                DisplayMenu((int)currentMenu);
                InitStore(menus[(int)currentMenu], currentMenu);
                break;

            case UpgradeMenu.WeaponUpgradeMenu:
                SwitchCamera((int)currentMenu);
                yield return new WaitForSeconds(Delay / 2);
                DisplayMenu((int)currentMenu);
                InitStore(menus[(int)currentMenu], currentMenu);
                break;

            case UpgradeMenu.AbilityUpgradeMenu:
                EnablePrompt = false;
                SwitchCamera(0);
                UIEvents.OnStoreMenuOpened?.Invoke();
                yield return new WaitForSeconds(Delay);
                DisplayMenu(0);
                InitStore(menus[0], currentMenu);
                break;

            case UpgradeMenu.OfferingSelectionMenu:
                EnablePrompt = false;
                yield return new WaitForSeconds(Delay);
                DisplayMenu(0);
                InitStore(menus[0], currentMenu);
                break;

            default:
                break;
        }
        StopCoroutine("Enter_State");
    }
    public void ExitState(int state)
    {
        StartCoroutine(Exit_State(state));   
    }
    private IEnumerator Exit_State(int state)
    {
        switch ((UpgradeMenu)state)
        {
            case UpgradeMenu.StoreSelectionMenu:
                HideMenu(state);
                if (closeMenu)
                {
                    yield return new WaitForSeconds(.25f);
                    SwitchToMainCamera();
                    yield return new WaitForSeconds(.5f);
                    PlayerController.instance.EnableInput();
                    closeMenu = false;
                    EnablePrompt = true;
                    _inMenu = false;
                    UIEvents.OnStoreMenuClosed?.Invoke();
                }
                break;

            case UpgradeMenu.WeaponPurchaseMenu:
                HideMenu(state);
                HideMenu(menus.Length - 1);
                EnterState((int)UpgradeMenu.StoreSelectionMenu);
                break;  

            case UpgradeMenu.WeaponUpgradeMenu:
                GetComponent<WeaponUpgradeStore>().ResetMenu();
                HideMenu(state);
                HideMenu(menus.Length - 1);
                EnterState((int)UpgradeMenu.StoreSelectionMenu);
                break;

            case UpgradeMenu.AbilityUpgradeMenu:
                HideMenu(0);
                yield return new WaitForSeconds(.25f);
                SwitchToMainCamera();
                closeMenu = false;
                yield return new WaitForSeconds(.5f);
                EnablePrompt = true;
                PlayerController.instance.EnableInput();
                break;

            default:
                break;

            case UpgradeMenu.OfferingSelectionMenu:
                HideMenu(0);
                HideMenu(1);
                yield return new WaitForSeconds(.25f);
                SwitchToMainCamera();
                closeMenu = false;
                yield return new WaitForSeconds(.5f);
                EnablePrompt = true;
                PlayerController.instance.EnableInput();
                break;

        }
        StopCoroutine("Exit_State");
    }   
    private void SwitchCamera(int index)
    {
        if (menuCameras[0] != null)
        {
            CameraManager.instance.GetCurrentCam().Priority = 0;
            menuCameras[index].Priority = 12;
            CameraManager.instance.SetCamera(menuCameras[index]);
        }
    }
    private void SwitchToMainCamera()
    {
        CameraManager.instance.MainCam.Priority = 12;
        CameraManager.instance.GetCurrentCam().Priority = 10;
        CameraManager.instance.SetCamera(CameraManager.instance.MainCam);
    }
    private void HideMenu(int index)
    {
        CanvasGroup menu = menus[index].GetComponent<CanvasGroup>();
        // play some buttton animation 
        if (UIEvents.OnMenuClosed != null)
            UIEvents.OnMenuClosed(menu);
        else
            menu.alpha = 0;
        menus[index].SetActive(false);
    }
    private void DisplayMenu(int index)
    {        
        CanvasGroup menu = menus[index].GetComponent<CanvasGroup>();
        // play some enter menu transition animation 
        menus[index].SetActive(true);

        // change alpha value of canvas group here.
        if (UIEvents.OnMenuOpened != null)
            UIEvents.OnMenuOpened(menu);
        else
            menu.alpha = 1;
    }
    private void InitStore(GameObject menu, UpgradeMenu _menu)
    { 
        switch (_menu)
        {
            case UpgradeMenu.StoreSelectionMenu:
                break;

            case UpgradeMenu.WeaponPurchaseMenu:
                GetComponent<WeaponStore>().InitWeaponStore(menu);
                break;

            case UpgradeMenu.WeaponUpgradeMenu:
                GetComponent<WeaponUpgradeStore>().InitWeaponUpgradeStore(menu);
                break;

            case UpgradeMenu.AbilityUpgradeMenu:
                GetComponent<AttributeUpgradeStore>().InitAttributeUpgradeStore(menu);
                break;

            default:
                break;
        }
    }
    #endregion
}
