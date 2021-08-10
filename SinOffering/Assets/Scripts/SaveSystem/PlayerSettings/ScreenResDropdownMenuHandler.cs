using UnityEngine;
using TMPro;

public class ScreenResDropdownMenuHandler : MonoBehaviour
{
    public TMP_Dropdown dropDown;
    public PlayerSettings playerSettings; 
    
    // Start is called before the first frame update
    void Start()
    {
        dropDown.onValueChanged.AddListener(delegate
        {
            ScreenResDDValueChanged(dropDown);
        });
    }

    public void ScreenResDDValueChanged(TMP_Dropdown sender)
    { 
        playerSettings.SetScreenResolution(sender.value); 
    }
}
