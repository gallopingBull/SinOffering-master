using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine;

public class AttributeUpgradeStore : MonoBehaviour
{
    private IAttributeStoreCustomer customer;
    private Dictionary<string, AttributeData> attributeDatabase;
    public Button[] menuButtons = null;
    public GameObject menu = null;
    private GameObject faithValueUI;
    private GameObject silverValueUI;
    private HUDManager hm;

    [Tooltip("Max Faith ammount required to unlock last modfications.")]
    [SerializeField] 
    private float maxFaithReq = 25000f;

    public int BaseRespecCost = 250;
    public int AdjustRespecCost = 0;
    //private int totalFaithValue = 0;
    public TextMeshProUGUI respecCost_Text; // displays respec cost
    public Image totalFaithSpentProgressBar; // white fill bar displaying TotalFaithAccrued
    public Image unlockedFaithProgressBar; // red fill bar displaying last unlocked 

    //[SerializeField]
    public Image buttonprogressBar; // reference to radial image arround buttons to indicate status of hold
    private float chargeTimer = 0;
    [SerializeField]    
    private float chargeTimeMax = 3;

    private bool _respecPurchased = false;
    
    void Start()
    {
        attributeDatabase = AttributeDatabase._instance.GetAttributeDatabase();
        faithValueUI = GameObject.Find("Text_FaithValue");
        silverValueUI = GameObject.Find("Text_SilverValue");
        hm = HUDManager._instance;
        
        #region testing
        //respecCost_Text = GameObject.Find("Text_RespecPrice").GetComponent<TextMeshProUGUI>();
        //currentFaithProgressBar = GameObject.Find("Panel_ProgressBar_Current").GetComponent<Image>();
        //unlockedFaithProgressBar = GameObject.Find("Panel_ProgressBar_Unlocked").GetComponent<Image>();
        #endregion
    }   
    private void Update()
    {
        if (Input.GetKey(KeyCode.T))
        {
            if (AdjustRespecCost > GameManager.instance.TotalSilver)
                return;

            chargeTimer += Time.deltaTime;
            buttonprogressBar.fillAmount = (chargeTimer / chargeTimeMax) * 1f;

            //Debug.Log("chargeTimer: " + chargeTimer);
            if (chargeTimer >= chargeTimeMax)
            {
                RespecAttributes();
                chargeTimer = 0.0f;
            }
        }
        if (Input.GetKeyUp(KeyCode.T))
        {
            buttonprogressBar.fillAmount = 0;
            chargeTimer = 0.0f;
        }
    }

    public void InitAttributeUpgradeStore(GameObject _menu)
    {
        // buttons for weapon or upgrades that haven't been purchased yet
        // should be made interactable reading in data from inventory.CS ...
        List<KeyValuePair<string, AttributeData>> _upgradeTypes = attributeDatabase.ToList();

        // switch out and get these levels from players ref to savesystem
        Dictionary<AttributeUpgradeTypes.UpgradeType, int> attributeUpgradeLevels =
            PlayerController.instance.Attributes.GetAttributeLevelData();

        // list of all buttons in Attribute Upgrade Menu
        if (menuButtons.Length == 0)
            SetMenuButtons(_menu);

        var sortedList = menuButtons.OrderBy(o => (int)o.gameObject.GetComponent<AttributeUpgradeButton>().UpgradeType);
        
        for (int i = 0; i < sortedList.ToList().Count; i++)
        {
            var _button = sortedList.ToList()[i].gameObject.GetComponent<AttributeUpgradeButton>();
            int tmpLevel = attributeUpgradeLevels[_button.UpgradeType];

            // assign child button
            if(i >= 0 && i < sortedList.ToList().Count - 1)
            {
                if (!_button.HasChildren)
                {
                    if (_button.UpgradeType == sortedList.ToList()[i + 1].gameObject.GetComponent<AttributeUpgradeButton>().UpgradeType)
                        _button.Child_Button = sortedList.ToList()[i + 1];
                } 
            }

            AttributeUpgradeData attributeData = _upgradeTypes[(int)_button .UpgradeType].Value.AttributeDataList[_button.UpgradeLevel];
            InitButton(_button, attributeData, _button.UpgradeType.ToString(), tmpLevel);
        }

        AdjustRespecCost = GetRespecCost();
        respecCost_Text.text = AdjustRespecCost.ToString();
    
        if (!_respecPurchased)
            SetProgressBarFillAmmount();
        
    }

    // initilaize buttons with correct weapon data 
    public void InitButton(AttributeUpgradeButton _button, AttributeUpgradeData _attributeData, string _upgradeName, int curUpgradeLvl)
    {
        _button.SetButtonData(attributeDatabase[_button.UpgradeType.ToString()], curUpgradeLvl);
        if (!_button.UpgradePurchased)
        {
            _button.gameObject.GetComponent<AttributeUpgradeButton>().OnLongClick.RemoveAllListeners();
            _button.gameObject.GetComponent<AttributeUpgradeButton>().OnLongClick.AddListener(() =>
            PurchaseUpgrade(_upgradeName, curUpgradeLvl, _button.GetComponent<Button>()));
        }
        else
        {
            _button.isDirty = true;
        }
    }

    public void PurchaseUpgrade(string _upgradeType, int _upgradeLevel, Button _button)
    {
        AttributeData tmpData = attributeDatabase[_upgradeType];
        int price = tmpData.AttributeDataList[_upgradeLevel].AttributePrice; // get price
        //Debug.Log("price: " + price + " || upgrade level: " + _upgradeLevel + " || upgradeType: " + _upgradeType);

        var playerAttributeLevels = PlayerController.instance.Attributes.GetAttributeLevelData();

        int maxLevel = tmpData.AttributeDataList.Length;
        if (customer.CanPurchaseUpgrade(price) && !_button.GetComponent<AttributeUpgradeButton>().UpgradePurchased)
        {
            //Debug.Log("maxLevel: " + maxLevel + " || playerAttributeLevels" + playerAttributeLevels[tmpData.UpgradeType]);
            if (playerAttributeLevels[tmpData.UpgradeType] < maxLevel)
            {
                if (!_button.GetComponent<AttributeUpgradeButton>().isDirty)
                    GameManager.instance.IncrementFaithSpent(price); // check if level/button is dirty (previously purchased)

                customer.PurchaseUpgrade(tmpData.UpgradeType);
                _button.interactable = false;

                _button.GetComponent<AttributeUpgradeButton>().PurchaseUpgrade(); // changes button faith
                faithValueUI.GetComponent<DisplayFaithTotal>().SetFaithValueUI(); // change to faith
                hm.SetUIObjectValues();

                InitAttributeUpgradeStore(GetComponent<MenuManager>().menus[0]); // reinitialize store and buttons
                SelectNextButton(_button);
            }
        }
    }

    private void SelectNextButton(Button _button)
    {
        for (int i = 0; i < menuButtons.Length; i++)
        {
            if (_button == menuButtons[i])
            {
                EventSystem.current.SetSelectedGameObject(null);
                if (i < menuButtons.Length - 1)
                    EventSystem.current.SetSelectedGameObject(menuButtons[i + 1].gameObject);
                else
                    EventSystem.current.SetSelectedGameObject(menuButtons[0].gameObject);
            }
        }
    }
    
    private void SetMenuButtons(GameObject _menu)
    {
        //Debug.Log("SetMenuButtons("+ _menu.name+ ")");
        menu = _menu;
        menuButtons = _menu.GetComponentsInChildren<Button>();
        faithValueUI = GameObject.Find("Text_FaithValue");
        silverValueUI = GameObject.Find("Text_SilverValue");
    }

    // customer/player is assigned at store trigger (DisplayButton.cs)
    public void AssignCustomer(IAttributeStoreCustomer _customer)
    {
        customer = _customer;
    }

    private int GetRespecCost()
    {
        int purchasedCount = 0;
        for (int i = 0; i < menuButtons.Length; i++)
        {
            if (menuButtons[i].GetComponent<AttributeUpgradeButton>().UpgradePurchased)
                purchasedCount++;
        }
        Debug.Log("purchasedCount: " + purchasedCount);
        return purchasedCount * BaseRespecCost;
    }

    private void RespecAttributes()
    {
        _respecPurchased = true;
        int tmpFaith = 0;
        int price = 0;


        AdjustRespecCost = GetRespecCost();
        GameManager.instance.TotalSilver -= AdjustRespecCost;
        AttributeData tmpData;

        for (int i = 0; i < menuButtons.Length; i++)
        {
            if (menuButtons[i].GetComponent<AttributeUpgradeButton>().UpgradePurchased)
            {
                // store prices for every purchased ugrade
                tmpData = attributeDatabase[menuButtons[i].GetComponent<AttributeUpgradeButton>().UpgradeType.ToString()];
                // get price
                price = tmpData.AttributeDataList[menuButtons[i].GetComponent<AttributeUpgradeButton>().UpgradeLevel].AttributePrice;
                tmpFaith += price;

                // reset buttons to default state/values
                PlayerController.instance.Attributes = new PlayerAttributes();
            }
        }
     
        InitAttributeUpgradeStore(menu);
        // add tmpFaith to totalFaith in game manager
        GameManager.instance.TotalCurrentFaith += tmpFaith;

        hm.SetUIObjectValues();
        faithValueUI.GetComponent<DisplayFaithTotal>().SetFaithValueUI();
        silverValueUI.GetComponent<DisplaySilverTotal>().SetSilverValueUI();
        _respecPurchased = false; 
    }

    private void SetProgressBarFillAmmount()
    {
        totalFaithSpentProgressBar.fillAmount = ((float)GameManager.instance.TotalFaithSpent / maxFaithReq) * 1f;
        if (totalFaithSpentProgressBar.fillAmount < .33)
            unlockedFaithProgressBar.fillAmount = 0;
        else if (totalFaithSpentProgressBar.fillAmount < .67)
            unlockedFaithProgressBar.fillAmount = .33f;
        else if (totalFaithSpentProgressBar.fillAmount < 1)
            unlockedFaithProgressBar.fillAmount = .67f;
        else
            unlockedFaithProgressBar.fillAmount = 1;
    }
}
