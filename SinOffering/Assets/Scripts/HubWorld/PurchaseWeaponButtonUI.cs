using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PurchaseWeaponButtonUI : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerUpHandler, IPointerDownHandler
{
    public string ItemName;

    // UI Elements
    [HideInInspector]
    public TextMeshProUGUI ItemName_Text;
    [HideInInspector]
    public TextMeshProUGUI Price_Text;
    [HideInInspector]
    public Button item_Button;


    private bool isSelected = false;
    public Image buttonFillImage;


    private float chargeTimer = 0;
    [SerializeField]
    private float chargeTimeMax = 3;

    private void Start()
    {
        item_Button = GetComponent<Button>();
        ItemName_Text = transform.Find("Text_WeaponName").GetComponent<TextMeshProUGUI>(); 
        Price_Text = transform.Find("Text_WeaponPrice").GetComponent<TextMeshProUGUI>();
    }
        /*
    private void Update()
    {
        if (isSelected)
        {
            if (Input.GetKey(KeyCode.Space) || Input.GetButton("Jump"))
            {
                chargeTimer += Time.deltaTime;

                //Debug.Log("chargeTimer: " + chargeTimer);
                if (chargeTimer >= chargeTimeMax)
                {
                    //if (OnLongClick != null)
                        //OnLongClick.Invoke();

                    //ResetButtonPressedTimer();
                }
                buttonFillImage.fillAmount = (chargeTimer / chargeTimeMax) * 1f;
            }

            if (Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("Jump"))
            {
                buttonFillImage.fillAmount = 0;
                chargeTimer = 0;
            }
        }
    }*/

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    void IDeselectHandler.OnDeselect(BaseEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
