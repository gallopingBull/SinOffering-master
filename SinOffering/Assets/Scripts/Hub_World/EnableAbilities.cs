//toggle player's abilities (jump/double jump/dash/ etc.) 
//in certain locations.

using UnityEngine;

public class EnableAbilities : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            PlayerController.instance.AbilitiesEnabled = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            PlayerController.instance.AbilitiesEnabled = true;
    }
}