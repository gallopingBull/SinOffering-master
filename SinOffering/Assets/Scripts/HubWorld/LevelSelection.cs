using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Cinemachine;
using UnityEngine;

// Enable Level selection menu
public class LevelSelection : MonoBehaviour
{
    private GameManager gameManager;
    public CinemachineVirtualCamera menuCamera;
    public GameObject menu;

    void Start()
    {
        gameManager = GameManager.instance;
    }
    
    private void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            PlayerController.instance.EnableInput();
            // play some buttton animation 
            // play some exit menu transition animation 
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        PlayerController.instance.DisableInput();

        if (menuCamera != null)
        {
            //CameraManager.instance.GetCurrentCam().Priority = 10;
            menuCamera.Priority = 12;
            CameraManager.instance.SetCamera(menuCamera);
        }
        // play some enter menu transition animation 
    }

    private void OnDisable()
    {
        // switch back to main camera (maybe move this back to cam manager)
        if (menuCamera != null)
        {
            CameraManager.instance.MainCam.Priority = 12;
            CameraManager.instance.GetCurrentCam().Priority = 10;
            CameraManager.instance.SetCamera(CameraManager.instance.MainCam);
        }
    }
}
