using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [HideInInspector]
    public static HUDManager _instance;

    private PlayerController pc; 
    

    public GameObject HUD; // reference to GO that UI Canvas is nested in
    // reference to canvas group alpha value for HUD gameobject
    public CanvasGroup hud_CanvasGroup;

    // reference to health bar UI
    public Image healthBar;
    // reference to health value from playercontroller
    private float healthValue;

    // reference to mana bar UI
    public Image manaBar;
    // reference to mana value from playercontroller
    private float manaValue;

    // reference to silver text UI
    public TextMeshProUGUI silverText;
    // reference to silver value from game manager
    private int silverValue = 0;


    // reference to faith text UI
    public TextMeshProUGUI faithText;
    // reference to faith value from game manager
    private int faithValue = 0;


    //private FadeCanvasGroup fadeCanvas;

    private Scene currentScene;

    private void Awake()
    {
        if (_instance != null)
            return;
        _instance = this;
        //fadeCanvas = FadeCanvasGroup._instance;
        currentScene = SceneManager.GetActiveScene();
    }
    void Start()
    {
        HUD = transform.Find("HUD").gameObject;
        pc = PlayerController.instance;
        hud_CanvasGroup = GetComponent<CanvasGroup>();
        SetUIObjects();
        SetUIObjectValues();
        healthBar = GameObject.Find("HealthBar_Fill").GetComponent<Image>();
        manaBar = GameObject.Find("ManaBar_Fill").GetComponent<Image>();
        
        if (currentScene.name == "HubScene")
            HideHUD();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
            ToggleHUD();
    }

    private void SetUIObjects()
    {
        healthBar = GameObject.Find("HealthBar_Fill").GetComponent<Image>();
        manaBar = GameObject.Find("ManaBar_Fill").GetComponent<Image>();
        silverText = GameObject.Find("Text_Silver_HUD").GetComponent<TextMeshProUGUI>();
        faithText = GameObject.Find("Text_FaithTotal_HUD").GetComponent<TextMeshProUGUI>();
    }
    public void SetUIObjectValues()
    {
        healthValue = pc.Health / 100; // change 100 to maxHealth
        healthBar.fillAmount = healthValue;

        manaValue = pc.Mana;
        manaBar.fillAmount = manaValue / 100; // change 100 to maxMana

        silverValue = GameManager.instance.TotalSilver;
        silverText.text = silverValue.ToString();

        faithValue = GameManager.instance.TotalCurrentFaith;
        faithText.text = faithValue.ToString();
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

    public void UpdateHealthBar(float value)
    {
        healthValue = pc.Health / 100;// change 100 to maxHealth
        healthBar.fillAmount = healthValue;
    }

    void UpdateManaBar()
    {
        //manaValue = pc.Mana;
        //manaBar.fillAmount = manaValue / 200; // change 100 to maxMana

    }
    void UpdateSilverValue()
    {

    }
    void UpdateFaithValue()
    {

    }
}
