using System.Collections.Generic;
using System.Collections;
using Cinemachine;
using UnityEngine.UI;

using UnityEngine;

//weapon/ability menu manager
public class MenuManager : MonoBehaviour
{
    [HideInInspector]
    public bool EnablePrompt = true;
    [HideInInspector]
    public enum UpgradeMenu
    {
        StoreSelectionMenu,
        WeaponPurchaseMenu,
        WeaponUpgradeMenu,
        AbilityUpgradeMenu,
    }

    [HideInInspector]
    public UpgradeMenu currentMenu;
    public GameObject[] menus;
    public CinemachineVirtualCamera[] menuCameras;

    [SerializeField]
    private float Delay = 1f;
    private bool closeMenu = false;


    private Dictionary<string, WeaponData> weaponDatabase;
    private WeaponData weaponData;


    private void Start()
    {
        weaponDatabase = Database._instance.GetWeaponDatabase();
    }
    private void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            //this gets executed twice with weapon upgrade store.
            Debug.Log("press back");
            Debug.Log("closeMenu (precondition) = " + closeMenu);
            if ((int)currentMenu == 0 
                && menus[0].activeInHierarchy)
            {
                closeMenu = true;
                Debug.Log("closeMenu (post-condition) = " + closeMenu);
            }
                
            ExitState((int)currentMenu);
        }
    }


    public void EnterState(int state)
    {
        PlayerController.instance.DisableInput();

        currentMenu = (UpgradeMenu)state;
        Debug.Log("enterstate() || state = " + (UpgradeMenu)state);
        StartCoroutine("Enter_State");
    }

    private IEnumerator Enter_State()
    {
        switch (currentMenu)
        {
            case UpgradeMenu.StoreSelectionMenu:
                EnablePrompt = false;
                SwitchCamera((int)currentMenu);
                yield return new WaitForSeconds(Delay/2);
                DisplayMenu((int)currentMenu);
                break;

            case UpgradeMenu.WeaponPurchaseMenu:
                SwitchCamera((int)currentMenu);
                yield return new WaitForSeconds(Delay);
               
                DisplayMenu((int)currentMenu);
                DisplayMenu(menus.Length - 1); // this displays UI for player's silver

                InitStore(menus[(int)currentMenu], currentMenu);
                break;

            case UpgradeMenu.WeaponUpgradeMenu:
                SwitchCamera((int)currentMenu);
                yield return new WaitForSeconds(Delay / 2);
                
                DisplayMenu((int)currentMenu);
                DisplayMenu(menus.Length - 1); // this displays UI for player's silver

                InitStore(menus[(int)currentMenu], currentMenu);
                break;

            case UpgradeMenu.AbilityUpgradeMenu:
                EnablePrompt = false;
                SwitchCamera(0);
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
                Debug.Log("exit state - storeSelectionMenu");
                HideMenu(state);
                if (closeMenu)
                {
                    yield return new WaitForSeconds(.25f);
                    SwitchToMainCamera();
                    yield return new WaitForSeconds(.5f);
                    PlayerController.instance.EnableInput();
                    closeMenu = false;
                    EnablePrompt = true;
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
        }
        StopCoroutine("EXit_State");
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
        //play some buttton animation 
        //play some exit menu transition animation 
        menus[index].SetActive(false);
    }
    private void DisplayMenu(int index)
    {
        //play some enter menu transition animation 
        menus[index].SetActive(true);
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
                //GetComponent<WeaponUpgradeStore>().InitWeaponUpgradeStore(menu);
                break;

            default:
                break;
        }
    }
}
