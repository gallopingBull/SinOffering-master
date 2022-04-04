using UnityEngine;

/// <summary>
/// helper class for GateBehavior.cs to detect collision for door objects.
/// </summary>

public class GateTrigger : MonoBehaviour
{
    public bool EnableGate = false;
    public bool IsOfferingGate = false;
    private GateBehavior _gate;
    private GameManager _gameManager;
    
    private void Start()
    {
        _gate = GetComponentInParent<GateBehavior>();
        _gameManager = GameManager.Instance;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Player")
        {
            if (IsOfferingGate)
                EnableGate = _gameManager.GameModeSelected;
            if (EnableGate)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    PlayerController.instance.AbilitiesEnabled = false;
                    PlayerController.instance.InputEnabled = false;
                    _gate.Toggle();
                }
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
