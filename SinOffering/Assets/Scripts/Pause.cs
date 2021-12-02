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
        // add some bool in base class for menus when theyre genericed so they can get checked of theyrective all at once.
        if (gm.GameWonPanel != null)
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
            return;
        }
        else
        {

            if (!gm.paused)
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
                gm.paused = false;
                Time.timeScale = 1f;
                PlayerController.instance.EnableInput();
                gm.pauseMenu.SetActive(false);
            }
        }

    }

}
