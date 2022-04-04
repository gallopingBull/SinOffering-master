using TMPro;
using UnityEngine;

/// <summary>
/// gets and sets faith value into store UI menus. 
/// </summary>

public class DisplayFaithTotal : MonoBehaviour
{
    private TextMeshProUGUI _faithTotal_Text;
    private GameManager _gameManager;

    // Update is called once per frame
    void Awake()
    {
        _gameManager = GameManager.Instance;
        _faithTotal_Text = GetComponent<TextMeshProUGUI>();
    }
    
    private void OnEnable() => SetFaithValueUI();
    
    public void SetFaithValueUI()
    {
        _faithTotal_Text.text = _gameManager.TotalCurrentFaith.ToString();
    }
}
