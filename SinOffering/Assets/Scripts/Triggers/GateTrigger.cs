using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateTrigger : MonoBehaviour
{
    public bool enableGate = false;
    public bool isOfferingGate = false;
    private GateBehavior _gate;

    private void Start()
    {
        _gate = GetComponentInParent<GateBehavior>();
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Player")
        {

            if (isOfferingGate)
                enableGate = GameManager.instance.gameModeSelected;
            if (enableGate)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    PlayerController.instance.AbilitiesEnabled = false;
                    PlayerController.instance.InputEnabled = false;
                    _gate.Toggle();
                }
            }
            else
            {


            }
        }
    }



    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            _gate.CloseGate();
            PlayerController.instance.AbilitiesEnabled = true;
            PlayerController.instance.InputEnabled = true;
        }
    }
}
