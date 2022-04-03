using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

// manager class that handles singleton instance of player HUD
public class HUDManager : MonoBehaviour
{
    #region variables
    [HideInInspector]
    public static HUDManager _instance;

    private PlayerController pc;

    // reference to canvas group alpha value for HUD gameobject.
    public CanvasGroup hud_CanvasGroup;

    // reference to health bar UI.
    [SerializeField] Image _healthBar;
    // reference to health value from playercontroller.
    private float _healthValue;

    // reference to mana bar UI
    [SerializeField] Image _manaBar;
    // reference to mana value from playercontroller.
    private float _manaValue;

    // reference to silver text UI.
    [SerializeField] TextMeshProUGUI _silverText;
    // reference to silver value from game manager.
    private int silverValue = 0;

    // reference to faith text UI.
    [SerializeField] TextMeshProUGUI _faithText;
    // reference to faith value from game manager.
    private int _faithValue = 0;


    //private FadeCanvasGroup fadeCanvas;
    private Scene _currentScene;

    #endregion

    #region functions   
    private void Awake()
    {
        if (_instance != null)
            return;
        _instance = this;
        _currentScene = SceneManager.GetActiveScene();
    }
    private void OnEnable()
    {
        GameEvents.OnDamageEvent += HandleHealthBar;
        GameEvents.OnManaUpdateEvent += UpdateManaBar;
        GameEvents.OnCurrencyUpdateEvent += UpdateSilverValue;
        GameEvents.OnFaithUpdateEvent += UpdateFaithValue;

        UIEvents.OnStoreMenuOpened += HideHUD;
        UIEvents.OnStoreMenuClosed += DisplayHUD;
    }
    private void OnDisable()
    {
        GameEvents.OnDamageEvent -= HandleHealthBar;
        GameEvents.OnManaUpdateEvent -= UpdateManaBar;
        GameEvents.OnCurrencyUpdateEvent -= UpdateSilverValue;
        GameEvents.OnFaithUpdateEvent -= UpdateFaithValue;
        
        UIEvents.OnStoreMenuOpened -= HideHUD;
        UIEvents.OnStoreMenuClosed -= DisplayHUD;
    }
    void Start()
    {
        pc = PlayerController.instance;
        hud_CanvasGroup = GetComponent<CanvasGroup>();
        SetUIObjects();
        SetUIObjectValues();
        if (_currentScene.name == "HubScene")
            HideHUD();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
            ToggleHUD();
    }
    private void SetUIObjects()
    {
        _healthBar = GameObject.Find("HealthBar_Fill").GetComponent<Image>();
        _manaBar = GameObject.Find("ManaBar_Fill").GetComponent<Image>();
        _silverText = GameObject.Find("Text_Silver_HUD").GetComponent<TextMeshProUGUI>();
        _faithText = GameObject.Find("Text_FaithTotal_HUD").GetComponent<TextMeshProUGUI>();
    } 
    public void SetUIObjectValues()
    {   
        _healthValue = pc.Health / 100; // change 100 to maxHealth
        _healthBar.fillAmount = _healthValue;

        _manaValue = pc.Mana;
        _manaBar.fillAmount = _manaValue / 100; // change 100 to maxMana

        silverValue = GameManager.instance.TotalSilver;
        _silverText.text = silverValue.ToString();

        _faithValue = GameManager.instance.TotalCurrentFaith;
        _faithText.text = _faithValue.ToString();
    }
    void ToggleHUD()
    {
        if (hud_CanvasGroup.alpha == 0)
            DisplayHUD();
        else
            HideHUD();
    }
    private void DisplayHUD() 
    {
        if (UIEvents.OnHUDDisplay != null)
            UIEvents.OnHUDDisplay(hud_CanvasGroup);
        else
            hud_CanvasGroup.alpha = 1;
    }
    public void HideHUD()
    {
        if (UIEvents.OnHUDHide != null)
            UIEvents.OnHUDHide(hud_CanvasGroup);
        else
            hud_CanvasGroup.alpha = 0;           
    }
    public void HandleHealthBar(float value) => _healthBar.fillAmount =  value / 100;
    void UpdateManaBar(float value)
    {
        _manaValue = value;
        _manaBar.fillAmount = _manaValue / 100; // change 100 to maxMana
    }
    void UpdateSilverValue(int value) => _silverText.text = value.ToString();
    void UpdateFaithValue(int value) => _faithText.text = value.ToString();
    #endregion
}
