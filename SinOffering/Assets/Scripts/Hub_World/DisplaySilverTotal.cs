using TMPro;
using UnityEngine;

/// <summary>
/// gets and sets silver value into store's UI menu. 
/// </summary>

public class DisplaySilverTotal : MonoBehaviour
{
    private TextMeshProUGUI SilverValue_Text;
    private GameManager _gameManager;

    void Awake()
    { 
        SilverValue_Text = GetComponent<TextMeshProUGUI>();
        _gameManager = GameManager.Instance;
    }
    
    private void OnEnable() => SetSilverValueUI();
    
    public void SetSilverValueUI()
    {
        SilverValue_Text.text = _gameManager.TotalSilver.ToString();
    }
}
