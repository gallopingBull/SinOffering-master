using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class AttributeUpgradeStore : MonoBehaviour
{
    private IAttributeStoreCustomer customer;
    private Dictionary<string, AttributeData> attributeDatabase;
    public Button[] menuButtons = null;
    public GameObject menu = null;
    private GameObject faithValueUI;
    private HUDManager hm;

    [Tooltip("Total Faith needed to Unlock Modfications")]
    [SerializeField] 
    private float maxFaith = 25000f;

    private int totalFaithValue = 0;
    public Image currentFaithProgressBar;
    public Image unlockedFaithProgressBar;

    //[SerializeField]
    public Image buttonprogressBar; //referenve to radial image arround buttons to indicate status of hold
    private float chargeTimer = 0;
    [SerializeField]
    private float chargeTimeMax = 3;


    void Start()
    {
        attributeDatabase = AttributeDatabase._instance.GetAttributeDatabase();
        faithValueUI = GameObject.Find("Text_FaithValue");
        hm = HUDManager._instance;
        //faithProgressBar = GameObject.Find("Panel_ProgressBar_Current").GetComponent<Image>();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.T))
        {
            chargeTimer += Time.deltaTime;
            buttonprogressBar.fillAmount = (chargeTimer / chargeTimeMax) * 1f;
            Debug.Log("chargeTimer: " + chargeTimer);
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

            /*
            _button.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            _button.gameObject.GetComponent<Button>().onClick.AddListener(() => 
            PurchaseUpgrade(_upgradeName, curUpgradeLvl, _button.GetComponent<Button>()));
            */
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
                customer.PurchaseUpgrade(tmpData.UpgradeType);
                _button.interactable = false;

                _button.GetComponent<AttributeUpgradeButton>().PurchaseUpgrade(); // changes button faith
                faithValueUI.GetComponent<DisplayManaTotal>().SetSilverValueUI(); // change to faith
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
    }

    // customer/player is assigned at store trigger (DisplayButton.cs)
    public void AssignCustomer(IAttributeStoreCustomer _customer)
    {
        customer = _customer;
    }

    private void RespecAttributes()
    {
        int tmpFaith = 0;

        AttributeData tmpData;
        int price = 0;

        for (int i = 0; i < menuButtons.Length; i++)
        {
            if (menuButtons[i].GetComponent<AttributeUpgradeButton>().UpgradePurchased)
            {
                // store prices for every purchased ugrade

                tmpData = attributeDatabase[menuButtons[i].GetComponent<AttributeUpgradeButton>().UpgradeType.ToString()];
                price = tmpData.AttributeDataList[menuButtons[i].GetComponent<AttributeUpgradeButton>().UpgradeLevel].AttributePrice; // get price

                tmpFaith += price;

                // reset buttons to default state/values
                PlayerController.instance.Attributes = new PlayerAttributes();

                // reset player attributes to default/values
                InitAttributeUpgradeStore(menu);
            }
        }
        // add tmpFaith to totalFaith in game manager
        GameManager.instance.TotalFaith += tmpFaith;
        hm.SetUIObjectValues();
    }

    private void SetProgressBarFillAmmount()
    {
        currentFaithProgressBar.fillAmount = ((float)GameManager.instance.TotalFaithAccrued/maxFaith) * 1f;
        Debug.Log(currentFaithProgressBar.fillAmount);
        
        if (currentFaithProgressBar.fillAmount < .33)
            unlockedFaithProgressBar.fillAmount = 0;
        else if (currentFaithProgressBar.fillAmount < .67)
            unlockedFaithProgressBar.fillAmount = .33f;
        else if (currentFaithProgressBar.fillAmount < 1)
            unlockedFaithProgressBar.fillAmount = .67f;
        else
            unlockedFaithProgressBar.fillAmount = 1;

    }
    void ButtonHeldTimer() {

       

    }

}
