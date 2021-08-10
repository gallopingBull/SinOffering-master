using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class PlayerSettings : MonoBehaviour
{
    int resX, resY;
    int refreshRate;
    public bool fullScreenEnabled;
    public static StereoRenderingPath stereoRenderingPath;

    // Start is called before the first frame update
    void Start()
    {
        InitGraphics();
        //InitControls();
    }

    //called before game(main menu scene) is loaded
    public void InitGraphics()
    {
        //only call once the first time the game is booted
        //default graphics settings
        if (!PlayerPrefs.HasKey("ScreenRes"))
        {
            PlayerPrefs.SetInt("ScreenRes", 0);
            fullScreenEnabled = true;
            PlayerPrefs.SetInt("FullScreenEnabled", 1);

            PlayerPrefs.Save();
            //print("created new screen resolution key in PlayerPrefs");
        }
        else
        {
            //print("Loading graphic settings");
            LoadGraphics();
        }
    }

    public void SetScreenResolution(int resID)
    {
        switch (resID)
        {
            case 0:
                resX = 4096;
                resY = 2160;
                break;

            case 1:
                resX = 3840;
                resY = 2160;
                break;

            case 2:
                resX = 2560;
                resY = 1440;
                break;

            case 3:
                resX = 1920;
                resY = 1080;
                break;

            case 4:
                resX = 1600;
                resY = 900;
                break;

            case 5:
                resX = 1440;
                resY = 900;
                break;

            case 6:
                resX = 1366;
                resY = 768;
                break;

            case 7:
                resX = 1200;
                resY = 800;
                break;

            case 8:
                resX = 1024;
                resY = 768;
                break;

            case 9:
                resX = 1280;
                resY = 720;
                break;

            case 10:
                resX = 768;
                resY = 1024;
                break;

            case 11:
                resX = 640;
                resY = 480;
                break;

            case 12:
                resX = 480;
                resY = 360;
                break;

            default:
                break;
        }
        PlayerPrefs.SetInt("ScreenRes", resID);
        Screen.SetResolution(resX, resY, fullScreenEnabled);
        //Screen.SetResolution(resX, resY, fullScreenEnabled, refreshRate);
        PlayerPrefs.Save();
    }

    public void EnableFullScreenRemote(bool enable)
    {
        if (enable)
        {
            fullScreenEnabled = true;
            PlayerPrefs.SetInt("FullScreenEnabled", 1);
        }
        else
        {
            fullScreenEnabled = false;
            PlayerPrefs.SetInt("FullScreenEnabled", 0);
            SetScreenResolution(3);

        }

        Screen.SetResolution(resX, resY, fullScreenEnabled);
        PlayerPrefs.Save();
    }

    public void EnableFullScreen()
    {
        if (PlayerPrefs.GetInt("FullScreenEnabled") == 1)
        {
            fullScreenEnabled = true;
            PlayerPrefs.SetInt("FullScreenEnabled", 1);
        }
        else
        {
            fullScreenEnabled = false;
            PlayerPrefs.SetInt("FullScreenEnabled", 0);
        }

        Screen.SetResolution(resX, resY, fullScreenEnabled);
        PlayerPrefs.Save(); 
    }

    private void LoadGraphics()
    { 
        SetScreenResolution(PlayerPrefs.GetInt("ScreenRes"));
        EnableFullScreen();

        Screen.SetResolution(resX, resY, fullScreenEnabled);
    }

    //called from SoundManager.cs (SoundManager.instance)
    //after text/sliders are assigned to avoid any null reference errors
    //at the begininig of runtime
    public void InitSound()
    {   
        //only call once the first time the game is booted
        //and no save settings are found
        if (!PlayerPrefs.HasKey("MusicVolume") || 
            !PlayerPrefs.HasKey("SFXVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", .7f);
            PlayerPrefs.SetFloat("SFXVolume", .7f);

            SoundManager.instance.SetVolume(PlayerPrefs.GetFloat("MusicVolume"),
           PlayerPrefs.GetFloat("SFXVolume"));
        }

        SoundManager.instance.SetVolume(PlayerPrefs.GetFloat("MusicVolume"),
            PlayerPrefs.GetFloat("SFXVolume"));
        
        PlayerPrefs.Save();
    }
}
