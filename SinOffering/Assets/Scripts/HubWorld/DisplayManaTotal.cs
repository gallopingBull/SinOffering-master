using TMPro;
using UnityEngine;

// gets and assigns silver value into UI for stores. 
public class DisplayManaTotal : MonoBehaviour
{
    private TextMeshProUGUI manaTotal_Text;

    // Update is called once per frame
    void Awake()
    {
        manaTotal_Text = GetComponent<TextMeshProUGUI>();
    }

    public void SetSilverValueUI()
    {
        manaTotal_Text.text = GameManager.instance.TotalFaith.ToString();
    }

    private void OnEnable()
    {
        SetSilverValueUI();
    }

}
