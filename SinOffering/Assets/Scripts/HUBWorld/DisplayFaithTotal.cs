using TMPro;
using UnityEngine;

// gets and assigns faith value into store UIs. 
public class DisplayFaithTotal : MonoBehaviour
{
    private TextMeshProUGUI _faithTotal_Text;

    // Update is called once per frame
    void Awake()
    {
        _faithTotal_Text = GetComponent<TextMeshProUGUI>();
    }

    public void SetFaithValueUI()
    {
        _faithTotal_Text.text = GameManager.instance.TotalCurrentFaith.ToString();
    }

    private void OnEnable()
    {
        SetFaithValueUI();
    }

}
