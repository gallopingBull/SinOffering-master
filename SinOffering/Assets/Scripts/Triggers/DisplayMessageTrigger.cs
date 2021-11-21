using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayMessageTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject displayMessageUI; // will change this to a canvasgroupReference 
    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Player")
            displayMessageUI.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
            displayMessageUI.SetActive(false);
    }
}
