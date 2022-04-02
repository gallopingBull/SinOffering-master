using TMPro;
using UnityEngine;

/// <summary>
/// gets and sets silver value into UI for stores. 
/// </summary>

public class DisplaySilverTotal : MonoBehaviour
{
    private TextMeshProUGUI SilverValue_Text;

    void Awake()
    { 
        SilverValue_Text = GetComponent<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        SetSilverValueUI();
    }
    public void SetSilverValueUI()
    {
        SilverValue_Text.text = 
            GameManager.instance.TotalSilver.ToString();
    }
}
