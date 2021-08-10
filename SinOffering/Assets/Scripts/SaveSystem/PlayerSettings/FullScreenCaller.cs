using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FullScreenCaller : MonoBehaviour
{
    private PlayerSettings playerSettings;


    public void Awake()
    {
        playerSettings = GameObject.Find("Main Camera").GetComponent<PlayerSettings>();
        if (playerSettings.fullScreenEnabled)
        {
            GetComponent<Toggle>().isOn = true;
        }
        else
        {
            GetComponent<Toggle>().isOn = false;
        }
    }

    public void EnableFSCaller()
    {
        playerSettings.EnableFullScreenRemote(GetComponent<Toggle>().isOn);
    }
}
