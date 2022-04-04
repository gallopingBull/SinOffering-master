using UnityEngine;

/// <summary>
/// pause functionality that pauses game. 
/// </summary>

public class Pause : MonoBehaviour
{
    private GameManager _gm;

    private void Start()
    {
        _gm = GameManager.Instance; 
    }

    public void PauseGame()
    {
        // add some bool in base class for menus when theyre genericed so they can get checked of theyrective all at once.
        if (_gm.GameWonPanel != null)
        {
            if (!_gm.GameWonPanel.activeInHierarchy && !_gm.Paused)
            {
                print("paused");
                _gm.Paused = true;
                Time.timeScale = 0f;
                PlayerController.instance.DisableInput();
                _gm.pauseMenu.SetActive(true);
                //hud.SetActive(false);
            }
            else
            {
                if (_gm.Paused)
                {
                    _gm.Paused = false;
                    Time.timeScale = 1f;
                    PlayerController.instance.EnableInput();
                    _gm.pauseMenu.SetActive(false);
                    //hud.SetActive(true);
                }
            }
            return;
        }
        else
        {
            if (!_gm.Paused)
            {
                print("paused");
                _gm.Paused = true;
                Time.timeScale = 0f;
                PlayerController.instance.DisableInput();
                _gm.pauseMenu.SetActive(true);
                //hud.SetActive(false);
            }
            else
            {
                _gm.Paused = false;
                Time.timeScale = 1f;
                PlayerController.instance.EnableInput();
                _gm.pauseMenu.SetActive(false);
            }
        }
    }
}
