using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class Pause : MonoBehaviour
{
    private GameManager gm;


    private void Start()
    {
        gm = GameManager.instance; 
    }


    public void PauseGame()
    {

        if (!gm.GameWonPanel.activeInHierarchy && !gm.paused)
        {
            print("paused");
            gm.paused = true;
            Time.timeScale = 0f;
            PlayerController.instance.DisableInput();
            gm.pauseMenu.SetActive(true);
            //hud.SetActive(false);
        }

        else
        {
            if (gm.paused)
            {
                gm.paused = false;
                Time.timeScale = 1f;
                PlayerController.instance.EnableInput();
                gm.pauseMenu.SetActive(false);
                //hud.SetActive(true);
            }
        }
    }

}
