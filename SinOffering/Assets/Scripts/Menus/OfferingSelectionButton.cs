using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class OfferingSelectionButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerUpHandler, IPointerDownHandler
{
    #region variables
    private bool isSelected = false;
    [HideInInspector]
    public bool isDirty = false;

    //[HideInInspector]
    //public int UpgradeLevel = 0;

    private float chargeTimer = 0;
    [SerializeField]
    private float chargeTimeMax = 3;

    [HideInInspector]
    public string offeringTitle;
    public TextMeshProUGUI offeringTitle_Text;

    //[HideInInspector]
    public bool ButtonLocked = false;
    private bool buttonInit = false;
    private bool buttonHeldDown = false;

    private OfferingData _data;
    public GameMode offeringMode;

    private GameModeSelectionMenu gameModeSelectionMenu;

    private AttributeButtonState state = AttributeButtonState.hidden;
    [SerializeField]
    private AttributeButtonColorsTest colorState;

    public UnityEvent OnLongClick;
    public Image buttonFillImage;

    #endregion

    #region functions
    private void Awake()
    {
        gameModeSelectionMenu = GameObject.Find("OfferingSelectionTemplate").GetComponent<GameModeSelectionMenu>();
    }

    public void SetData(OfferingData data)
    {
        _data = data;
        offeringTitle_Text.text = data.offeringTitle;
    }
    void IDeselectHandler.OnDeselect(BaseEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        gameModeSelectionMenu.DisplayOfferingDesciption(_data);
    }

    #endregion
}
