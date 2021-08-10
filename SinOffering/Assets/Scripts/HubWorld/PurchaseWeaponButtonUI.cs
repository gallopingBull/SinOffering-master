using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PurchaseWeaponButtonUI : MonoBehaviour
{
    public string ItemName;

    //UI Elements
    [HideInInspector]
    public TextMeshProUGUI ItemName_Text;
    [HideInInspector]
    public TextMeshProUGUI Price_Text;
    [HideInInspector]
    public Button item_Button;

    private void Start()
    {
        item_Button = GetComponent<Button>();
        ItemName_Text = transform.Find("Text_WeaponName").GetComponent<TextMeshProUGUI>(); 
        Price_Text = transform.Find("Text_WeaponPrice").GetComponent<TextMeshProUGUI>();
    }
}
    