using TMPro;
using UnityEngine;

// gets and assigns silver value into UI for stores. 
public class DisplaySilverTotal : MonoBehaviour
{
    private TextMeshProUGUI SilverValue_Text;

    // Update is called once per frame
    void Awake()
    { 
        SilverValue_Text = GetComponent<TextMeshProUGUI>();
    }

    public void SetSilverValueUI()
    {
        SilverValue_Text.text = 
            GameManager.instance.TotalSilver.ToString();
    }

    private void OnEnable()
    {
        SetSilverValueUI();
    }

}
