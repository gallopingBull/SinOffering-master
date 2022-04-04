using System.Collections.Generic;
using System.Collections;
using Cinemachine;
using UnityEngine;

/// <summary>
/// class that handles weapon/ability store menus
/// </summary>
///

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
    public UpgradeMenu CurrentMenu;
    public GameObject[] Menus;
    public CinemachineVirtualCamera[] MenuCameras;

    [SerializeField] float Delay = 1f;
    private bool _closeMenu = false;

    private Dictionary<string, WeaponData> _weaponDatabase;
    private WeaponData _weaponData;
    #endregion

    #region functions
    private void Start()
    {
        _weaponDatabase = WeaponDatabase._instance.GetWeaponDatabase();
    }
    
    private void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            // press back button while in menu this gets 
            // executed twice with weapon upgrade store.
            if ((int)CurrentMenu == 0 && Menus[0].activeInHierarchy)
                _closeMenu = true;

            ExitState((int)CurrentMenu);
        }
    }
    
    public void EnterState(int state)
    {
        PlayerController.instance.DisableInput();
        CurrentMenu = (UpgradeMenu)state;
        StartCoroutine("Enter_State");
    }
    
    private IEnumerator Enter_State()
    {
        switch (CurrentMenu)
        {
            case UpgradeMenu.StoreSelectionMenu:
                if (!_inMenu)
                {
                    _inMenu = true;
                    UIEvents.OnStoreMenuOpened?.Invoke();
                }
                EnablePrompt = false;
                SwitchCamera((int)CurrentMenu);
                yield return new WaitForSeconds(Delay/2);
                DisplayMenu((int)CurrentMenu);

                break;

            case UpgradeMenu.WeaponPurchaseMenu:
                SwitchCamera((int)CurrentMenu);
                yield return new WaitForSeconds(Delay);
                DisplayMenu((int)CurrentMenu);
                InitStore(Menus[(int)CurrentMenu], CurrentMenu);
                break;

            case UpgradeMenu.WeaponUpgradeMenu:
                SwitchCamera((int)CurrentMenu);
                yield return new WaitForSeconds(Delay / 2);
                DisplayMenu((int)CurrentMenu);
                InitStore(Menus[(int)CurrentMenu], CurrentMenu);
                break;

            case UpgradeMenu.AbilityUpgradeMenu:
                EnablePrompt = false;
                SwitchCamera(0);
                UIEvents.OnStoreMenuOpened?.Invoke();
                yield return new WaitForSeconds(Delay);
                DisplayMenu(0);
                InitStore(Menus[0], CurrentMenu);
                break;

            case UpgradeMenu.OfferingSelectionMenu:
                EnablePrompt = false;
                yield return new WaitForSeconds(Delay);
                DisplayMenu(0);
                InitStore(Menus[0], CurrentMenu);
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
                if (_closeMenu)
                {
                    yield return new WaitForSeconds(.25f);
                    SwitchToMainCamera();
                    yield return new WaitForSeconds(.5f);
                    PlayerController.instance.EnableInput();
                    _closeMenu = false;
                    EnablePrompt = true;
                    _inMenu = false;
                    UIEvents.OnStoreMenuClosed?.Invoke();
                }
                break;

            case UpgradeMenu.WeaponPurchaseMenu:
                HideMenu(state);
                HideMenu(Menus.Length - 1);
                EnterState((int)UpgradeMenu.StoreSelectionMenu);
                break;  

            case UpgradeMenu.WeaponUpgradeMenu:
                GetComponent<WeaponUpgradeStore>().ResetMenu();
                HideMenu(state);
                HideMenu(Menus.Length - 1);
                EnterState((int)UpgradeMenu.StoreSelectionMenu);
                break;

            case UpgradeMenu.AbilityUpgradeMenu:
                HideMenu(0);
                yield return new WaitForSeconds(.25f);
                SwitchToMainCamera();
                _closeMenu = false;
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
                _closeMenu = false;
                yield return new WaitForSeconds(.5f);
                EnablePrompt = true;
                PlayerController.instance.EnableInput();
                break;

        }
        StopCoroutine("Exit_State");
    }   
    
    private void SwitchCamera(int index)
    {
        if (MenuCameras[0] != null)
        {
            CameraManager.instance.GetCurrentCam().Priority = 0;
            MenuCameras[index].Priority = 12;
            CameraManager.instance.SetCamera(MenuCameras[index]);
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
        CanvasGroup menu = Menus[index].GetComponent<CanvasGroup>();
        // play some buttton animation 
        if (UIEvents.OnMenuClosed != null)
            UIEvents.OnMenuClosed(menu);
        else
            menu.alpha = 0;
        Menus[index].SetActive(false);
    }
    
    private void DisplayMenu(int index)
    {        
        CanvasGroup menu = Menus[index].GetComponent<CanvasGroup>();
        // play some enter menu transition animation 
        Menus[index].SetActive(true);

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
