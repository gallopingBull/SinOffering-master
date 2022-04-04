using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine;

/// <summary>
/// store class that allows player to purchase weapon upgrades.
/// </summary>

public class AttributeUpgradeStore : MonoBehaviour
{
    #region variables
    private IAttributeStoreCustomer _customer;
    private Dictionary<string, AttributeData> _attributeDatabase;
    public Button[] MenuButtons = null;
    public GameObject Menu = null;
    
    private GameObject _faithValueUI;
    private GameObject _silverValueUI;
    private HUDManager _hm;
    private GameManager _gameManager;

    [Tooltip("Max Faith ammount required to unlock last modfications.")]
    [SerializeField] 
    private float _maxFaithReq = 25000f;

    public int BaseRespecCost = 250;
    public int AdjustRespecCost = 0;
    //private int totalFaithValue = 0;
    public TextMeshProUGUI respecCost_Text; // displays respec cost
    public Image totalFaithSpentProgressBar; // white fill bar displaying TotalFaithAccrued
    public Image unlockedFaithProgressBar; // red fill bar displaying last unlocked 

    //[SerializeField]
    public Image ButtonProgressBar; // reference to radial image arround buttons to indicate status of hold
    private float _chargeTimer = 0;
    [SerializeField]    
    private float _chargeTimeMax = 3;

    private bool _respecPurchased = false;

    #endregion

    #region functions

    void Start()
    {
        _attributeDatabase = AttributeDatabase._instance.GetAttributeDatabase();
        _faithValueUI = GameObject.Find("Text_FaithValue");
        _silverValueUI = GameObject.Find("Text_SilverValue");
        _hm = HUDManager._instance;
        _gameManager = GameManager.Instance;
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
            if (AdjustRespecCost > _gameManager.TotalSilver)
                return;

            _chargeTimer += Time.deltaTime;
            ButtonProgressBar.fillAmount = (_chargeTimer / _chargeTimeMax) * 1f;

            //Debug.Log("chargeTimer: " + chargeTimer);
            if (_chargeTimer >= _chargeTimeMax)
            {
                RespecAttributes();
                _chargeTimer = 0.0f;
            }
        }
        if (Input.GetKeyUp(KeyCode.T))
        {
            ButtonProgressBar.fillAmount = 0;
            _chargeTimer = 0.0f;
        }
    }
    
    public void InitAttributeUpgradeStore(GameObject _menu)
    {
        // buttons for weapon or upgrades that haven't been purchased yet
        // should be made interactable reading in data from inventory.CS ...
        List<KeyValuePair<string, AttributeData>> _upgradeTypes = _attributeDatabase.ToList();

        // switch out and get these levels from players ref to savesystem
        Dictionary<AttributeUpgradeTypes.UpgradeType, int> attributeUpgradeLevels =
            PlayerController.instance.Attributes.GetAttributeLevelData();

        // list of all buttons in Attribute Upgrade Menu
        if (MenuButtons.Length == 0)
            SetMenuButtons(_menu);

        var sortedList = MenuButtons.OrderBy(o => (int)o.gameObject.GetComponent<AttributeUpgradeButton>().UpgradeType);
        
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
        _button.SetButtonData(_attributeDatabase[_button.UpgradeType.ToString()], curUpgradeLvl);
        if (!_button.UpgradePurchased)
        {
            _button.gameObject.GetComponent<AttributeUpgradeButton>().OnLongClick.RemoveAllListeners();
            _button.gameObject.GetComponent<AttributeUpgradeButton>().OnLongClick.AddListener(() =>
            PurchaseUpgrade(_upgradeName, curUpgradeLvl, _button.GetComponent<Button>()));
        }
        else
            _button.isDirty = true;
    }

    public void PurchaseUpgrade(string upgradeType, int upgradeLevel, Button button)
    {
        AttributeData tmpData = _attributeDatabase[upgradeType];
        int price = tmpData.AttributeDataList[upgradeLevel].AttributePrice; // get price
        //Debug.Log("price: " + price + " || upgrade level: " + _upgradeLevel + " || upgradeType: " + _upgradeType);

        var playerAttributeLevels = PlayerController.instance.Attributes.GetAttributeLevelData();

        int maxLevel = tmpData.AttributeDataList.Length;
        if (_customer.CanPurchaseUpgrade(price) && !button.GetComponent<AttributeUpgradeButton>().UpgradePurchased)
        {
            //Debug.Log("maxLevel: " + maxLevel + " || playerAttributeLevels" + playerAttributeLevels[tmpData.UpgradeType]);
            if (playerAttributeLevels[tmpData.UpgradeType] < maxLevel)
            {
                if (!button.GetComponent<AttributeUpgradeButton>().isDirty)
                    _gameManager.IncrementFaithSpent(price); // check if level/button is dirty (previously purchased)

                _customer.PurchaseUpgrade(tmpData.UpgradeType);
                button.interactable = false;

                button.GetComponent<AttributeUpgradeButton>().PurchaseUpgrade(); // changes button faith
                _faithValueUI.GetComponent<DisplayFaithTotal>().SetFaithValueUI(); // change to faith
                _hm.SetUIObjectValues();

                InitAttributeUpgradeStore(GetComponent<MenuManager>().Menus[0]); // reinitialize store and buttons
                SelectNextButton(button);
            }
        }
    }

    private void SelectNextButton(Button button)
    {
        for (int i = 0; i < MenuButtons.Length; i++)
        {
            if (button == MenuButtons[i])
            {
                EventSystem.current.SetSelectedGameObject(null);
                if (i < MenuButtons.Length - 1)
                    EventSystem.current.SetSelectedGameObject(MenuButtons[i + 1].gameObject);
                else
                    EventSystem.current.SetSelectedGameObject(MenuButtons[0].gameObject);
            }
        }
    }
    
    private void SetMenuButtons(GameObject menu)
    {
        //Debug.Log("SetMenuButtons("+ _menu.name+ ")");
        Menu = menu;
        MenuButtons = menu.GetComponentsInChildren<Button>();
        _faithValueUI = GameObject.Find("Text_FaithValue");
        _silverValueUI = GameObject.Find("Text_SilverValue");
    }

    // customer/player is assigned at store trigger (DisplayButton.cs)
    public void AssignCustomer(IAttributeStoreCustomer customer) => _customer = customer;

    private int GetRespecCost()
    {
        int purchasedCount = 0;
        for (int i = 0; i < MenuButtons.Length; i++)
            if (MenuButtons[i].GetComponent<AttributeUpgradeButton>().UpgradePurchased)
                purchasedCount++;
        
        //Debug.Log("purchasedCount: " + purchasedCount);
        return purchasedCount * BaseRespecCost;
    }

    private void RespecAttributes()
    {
        _respecPurchased = true;
        int tmpFaith = 0;
        int price = 0;


        AdjustRespecCost = GetRespecCost();
        _gameManager.TotalSilver -= AdjustRespecCost;
        AttributeData tmpData;

        for (int i = 0; i < MenuButtons.Length; i++)
        {
            if (MenuButtons[i].GetComponent<AttributeUpgradeButton>().UpgradePurchased)
            {
                // store prices for every purchased ugrade
                tmpData = _attributeDatabase[MenuButtons[i].GetComponent<AttributeUpgradeButton>().UpgradeType.ToString()];
                // get price
                price = tmpData.AttributeDataList[MenuButtons[i].GetComponent<AttributeUpgradeButton>().UpgradeLevel].AttributePrice;
                tmpFaith += price;

                // reset buttons to default state/values
                PlayerController.instance.Attributes = new PlayerAttributes();
            }
        }
     
        InitAttributeUpgradeStore(Menu); //re-initialize store after respec-ing
        // add tmpFaith to totalFaith in game manager
        _gameManager.TotalCurrentFaith += tmpFaith;

        _hm.SetUIObjectValues();
        _faithValueUI.GetComponent<DisplayFaithTotal>().SetFaithValueUI();
        _silverValueUI.GetComponent<DisplaySilverTotal>().SetSilverValueUI();
        _respecPurchased = false; 
    }

    private void SetProgressBarFillAmmount()
    {
        totalFaithSpentProgressBar.fillAmount = ((float)_gameManager.TotalFaithSpent / _maxFaithReq) * 1f;
        if (totalFaithSpentProgressBar.fillAmount < .33)
            unlockedFaithProgressBar.fillAmount = 0;
        else if (totalFaithSpentProgressBar.fillAmount < .67)
            unlockedFaithProgressBar.fillAmount = .33f;
        else if (totalFaithSpentProgressBar.fillAmount < 1)
            unlockedFaithProgressBar.fillAmount = .67f;
        else
            unlockedFaithProgressBar.fillAmount = 1;
    }

    #endregion
}
