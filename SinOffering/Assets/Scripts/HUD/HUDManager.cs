using System;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
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

    // reference to GO that UI Canvas is nested in.
    [SerializeField]
    private GameObject _hudGO;
    // reference to canvas group alpha value for HUD gameobject.
    public CanvasGroup hud_CanvasGroup;

    // reference to health bar UI.
    [SerializeField]
    private Image _healthBar;
    // reference to health value from playercontroller.
    private float _healthValue;

    // reference to mana bar UI
    [SerializeField]
    private Image _manaBar;
    // reference to mana value from playercontroller.
    private float _manaValue;

    // reference to silver text UI.
    [SerializeField]
    private TextMeshProUGUI _silverText;
    // reference to silver value from game manager.
    private int silverValue = 0;

    // reference to faith text UI.
    [SerializeField]
    public TextMeshProUGUI _faithText;
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

        //fadeCanvas = FadeCanvasGroup._instance;
        _currentScene = SceneManager.GetActiveScene();
    }
    void Start()
    {
        _hudGO = transform.Find("HUD").gameObject;
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

    private void OnEnable()
    {
        GameEvents.OnDamageEvent += HandleHealthBar;
    }
    private void OnDisable()
    {
        GameEvents.OnDamageEvent -= HandleHealthBar;
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
        SetUIObjectValues();
        GetComponent<FadeCanvasGroup>().FadeInCanvasGroup();
    }

    public void HideHUD()
    {
        GetComponent<FadeCanvasGroup>().FadeOutCanvasGroup();
    }

    public void HandleHealthBar(float value)
    {
        _healthValue = pc.Health / 100;// change 100 to maxHealth
        _healthBar.fillAmount = _healthValue;
    }

    void UpdateManaBar(float value)
    {
        _manaValue = pc.Mana;
        _manaBar.fillAmount = _manaValue / 100; // change 100 to maxMana
    }
    void UpdateSilverValue(int value)
    {
        silverValue = GameManager.instance.TotalSilver;
        _silverText.text = silverValue.ToString();

    }
    void UpdateFaithValue(int value)
    {
        _faithValue = GameManager.instance.TotalCurrentFaith;
        _faithText.text = _faithValue.ToString();
    }
    #endregion
}
