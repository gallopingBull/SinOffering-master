using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Entity, IWeaponStoreCustomer, IAttributeStoreCustomer
{
    #region variables

    public float Mana = 0;
    public float Strength = 0;



    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public static PlayerController instance;

    public bool dying = false;

    // PlayerController stats
    public float AirControl = 0; // keep value between 0 (no air control) and 1 (full air control)

    // handles input from user 
    public bool InputEnabled = true;
    [HideInInspector]
    public InputHandler inputHandler;
    private List<ICommand> commands;
    [SerializeField]
    private PlayerAttributes attributes = new PlayerAttributes();

    [HideInInspector]
    public bool AbilitiesEnabled = true; // turn off when player in areas that they can't use special abilities

    [HideInInspector]
    public bool CanDoubleJump = false;
    [HideInInspector]
    public bool JumpButtonHeldDown = false;

    //[HideInInspector]
    public float delay = 0f; // delay between jump input
    //[HideInInspector]
    public float MAX_delay = .3f; // delay between jump input
    //[HideInInspector]
    public bool delayComplete = true; 

    //weapons variable
    [HideInInspector]
    public GameObject EquippedWeapon;
    [HideInInspector]
    public WeaponManager weaponManager;

    [HideInInspector]
    public ParticleSystem ps;

    [HideInInspector]
    public float xRaw;
    [HideInInspector]
    public float yRaw;

    public AudioClip jumpClip, landClip, dashClip;
    //[HideInInspector]
    public GameObject button;

    [HideInInspector]
    public StateManager StateManager;
    [HideInInspector]
    public InputDelay InputDelay;

    public PlayerAttributes Attributes { get => attributes; set => attributes = value; }


    #endregion

    #region functions

    private void Awake() { instance = this; }
    // Update is called once per frame
    // ***USE ONLY FOR PHYSICS CALCULATIONS*** \\\
    protected override void FixedUpdate()
    {
        inputHandler.InputDelay();
        //InputDelay2();
        //InputDelay.InputDelayHandler(state); // manages delay timers for several different input/actions
        CheckIfFalling();
        GravityModifier();
        CheckFloor();
    }
  
    // ***USE ONLY FOR INPUT*** \\\
    private void Update()
    {
        xRaw = Input.GetAxisRaw("Horizontal");
        yRaw = Input.GetAxisRaw("Vertical");
        //InputDelay2();
        //InputDelay.InputDelayHandler(state); // manages delay timers for several different input/actions
        
        if (inputHandler)
            InputHandler();

        // spawn dust trails
        if (state == State.running && IsGrounded)
        {
            if (EnableDustTrails)
            {
                if (!canSpawnDustTrail)
                {
                    if (stepRate > 0)
                    {
                        stepRate -= Time.deltaTime;
                        return;
                    }
                    else
                    {
                        //print("spawn dust trail");
                        canSpawnDustTrail = true;
                        SpawnDustTrail();
                    }
                }
            }
        }
    }
    public void Test()
    {
        //print("Shit myself at Test() || PlayerController.cs || gameObject.name: " + transform.name);
    }

    protected override void InitActor()
    {
        StateManager = GetComponent<StateManager>();

        #region testing
        /*
        button = GameObject.Find("Image_Button");
        if(TestInfo.EnableDebugInfo){
            button.SetActive(false);
        }*/
        #endregion
        
        SetDefaultSpeed();
        animator = GetComponentInChildren<Animator>();
        inputHandler = GetComponent<InputHandler>();
        weaponManager = GetComponent<WeaponManager>();
        ps = GetComponent<ParticleSystem>(); 
        InputDelay = GetComponent<InputDelay>();
    }

    // Called when Player has been killed.
    public override void Killed()
    {
        if (gameManager != null && !isInvincible)
        {
            dying = true;
            if (GetComponent<DashCommand>().RadialMenu.activeInHierarchy)
                GetComponent<DashCommand>().DisableDashAttack();
            gameManager.FailedGame();
        }

        BloodActorSprite.gameObject.SetActive(false);
        DeParentCaller();
        Destroy(gameObject, .1f);
    }

    // handles command buffer
    private void InputHandler()
    {
        commands = inputHandler.HandleInput();
        
        if (commands != null)
        {
            for (int i = 0; i < commands.Count; i++)
                commands[i].Execute();
            commands.Clear();
        }
    }

    // delay thats prevents input spamming
    private void InputDelay2()
    {
        switch (state)
        {
            case Entity.State.Jumping:
                if (InputDelay.jumpDelay > 0f)
                {
                    InputDelay.jumpDelayComplete = false;
                    InputDelay.jumpDelay -= Time.fixedDeltaTime;
                    //if (InputDelay.jumpDelayComplete)
                }
                if (InputDelay.jumpDelay < 0f)
                {
                    InputDelay.jumpDelayComplete = true;
                    InputDelay.jumpDelay = 0;
                    //if (!InputDelay.jumpDelayComplete)     
                }
                break;
            case Entity.State.evading:
                if (InputDelay.evadeDelay > 0)
                {
                    if (InputDelay.evadeDelayComplete)
                        InputDelay.evadeDelayComplete = false;
                    InputDelay.evadeDelay -= Time.deltaTime;
                }
                if (InputDelay.evadeDelay < 0)
                {
                    if (!InputDelay.evadeDelayComplete)
                        InputDelay.evadeDelayComplete = true;
                    InputDelay.evadeDelay = 0;
                }
                break;

            case Entity.State.dashing:
                if (InputDelay.dashDelay > 0)
                {
                    if (InputDelay.dashDelayComplete)
                        InputDelay.dashDelayComplete = false;
                    InputDelay.dashDelay -= Time.deltaTime;
                }
                if (InputDelay.dashDelay < 0)
                {
                    if (!InputDelay.dashDelayComplete)
                        InputDelay.dashDelayComplete = true;
                    InputDelay.dashDelay = 0;
                }
                break;
            case Entity.State.falling:
                if (InputDelay.jumpDelay > 0)
                {
                    InputDelay.jumpDelayComplete = false;
                    InputDelay.jumpDelay -= Time.fixedDeltaTime;
                    //if (InputDelay.jumpDelayComplete);
                }
                break;
            /*
            case Entity.State.falling:
            case Entity.State.Idle:
            case Entity.State.running:
                if (InputDelay.jumpDelay > 0f)
                {
                    print("resseting jump");
                    InputDelay.jumpDelay = 0;
                    InputDelay.jumpDelayComplete = true;
                }
                if (InputDelay.evadeDelay > 0f)
                {
                    print("resseting evade");
                    InputDelay.evadeDelay = 0;
                    InputDelay.evadeDelayComplete = true;
                }
                if (InputDelay.dashDelay > 0f)
                {
                    print("resseting dash");
                    InputDelay.dashDelay = 0;
                    InputDelay.dashDelayComplete = true;
                }

            break;
            */
            default:
                if (InputDelay.jumpDelay > 0f)
                {
                    print("resseting jump");
                    InputDelay.jumpDelay = 0;
                    InputDelay.jumpDelayComplete = true;
                }
                if (InputDelay.evadeDelay > 0f)
                {
                    print("resseting evade");
                    InputDelay.evadeDelay = 0;
                    InputDelay.evadeDelayComplete = true;
                }
                if (InputDelay.dashDelay > 0f)
                {
                    print("resseting dash");
                    InputDelay.dashDelay = 0;
                    InputDelay.dashDelayComplete = true;
                }

                break;

        }

        /*
       if (delay > 0)
       {
           delayComplete = false;
           delay -= Time.deltaTime;
       }
       if (delay < 0)
       {
           delayComplete = true;
           delay = 0;
       }*/
    }

    public void EnableInput()
    {
        Debug.Log("enabling input");
        InputEnabled = true;
    }
    public void DisableInput()
    {
        Debug.Log("disabling input");
        InputEnabled = false;
    }


    public void SetPersistentPlayerAttributeData(PlayerAttributes _attributes)
    {
        attributes = _attributes;
        SetAttributeValues();
    }

    private void SetAttributeValues()
    {
        var dataBase = AttributeDatabase._instance.GetAttributeDatabase();
        Health = dataBase["health"].AttributeDataList[attributes.HealthAttributeLevel - 1].AttributeValue;//attributes.HealthAttributeLevel;//attributes.se
        Speed = dataBase["speed"].AttributeDataList[attributes.SpeedAttributeLevel].AttributeValue;//attributes.SpeedAttributeLevel;
        Mana = dataBase["mana"].AttributeDataList[attributes.ManaAttributeLevel - 1].AttributeValue ;//attributes.ManaAttributeLevel;//attributes.se
        Strength = dataBase["strength"].AttributeDataList[attributes.StrengthAttributeLevel].AttributeValue;//attributes.StrengthAttributeLevel;
    }


    // store functions (might want to move this interface in its own class,
    // and ill call a getter on it to get all nesecarry values)
    void IWeaponStoreCustomer.PurchaseWeapon(string _weaponName)
    {
        foreach (GameObject weapon in weaponManager.Weapons)
        {
            if (weapon.GetComponent<Weapon>().GetWeaponName() == _weaponName)
                weapon.GetComponent<Weapon>().WeaponAttributes.WeaponPurchased = true;
        }
        //Debug.Log("Purchased: " + _weaponName);
    }
    bool IWeaponStoreCustomer.CanPurchaseWeapon(int _price)
    {
        // check if already purchased or if
        // player has enough silver for weapon

        if (gameManager.TotalSilver < _price)
            return false;

        gameManager.TotalSilver -= _price;
        return true;

    }
    void IWeaponStoreCustomer.PurchaseWeaponUpgrade(string _weaponName, WeaponUpgradeTypes.UpgradeType upgradeType)
    {
        foreach (GameObject weapon in weaponManager.Weapons)
        {
            if (weapon.GetComponent<Weapon>().GetWeaponName() == _weaponName)
            {
                switch (upgradeType)    
                {           
                    case WeaponUpgradeTypes.UpgradeType.FireRate:
                        weapon.GetComponent<Weapon>().WeaponAttributes.fireRateLevel++;
                        break;
                    case WeaponUpgradeTypes.UpgradeType.WeaponDamage:
                        weapon.GetComponent<Weapon>().WeaponAttributes.WeaponDamageLevel++;
                        break;
                    case WeaponUpgradeTypes.UpgradeType.AmmoCapacity:
                        weapon.GetComponent<Weapon>().WeaponAttributes.AmmoCapacityLevel++;
                        break;
                    default:
                        break;
                }
            }
        }
    }
    bool IWeaponStoreCustomer.CanPurchaseWeaponUpgrade(int _price)
    {
        //Debug.Log("silver: " + gm.TotalSilver);
        if (gameManager.TotalSilver >= _price)
        {
            gameManager.TotalSilver -= _price;
            //Debug.Log("You have enough money to purchase");
            return true;
        }
        return false;
    }

    void IAttributeStoreCustomer.PurchaseUpgrade(AttributeUpgradeTypes.UpgradeType _upgradeType)
    {
        switch (_upgradeType)
        {
            case AttributeUpgradeTypes.UpgradeType.health:
                attributes.HealthAttributeLevel++;
                break;
            case AttributeUpgradeTypes.UpgradeType.mana:
                attributes.ManaAttributeLevel++;
                break;
            case AttributeUpgradeTypes.UpgradeType.strength:
                attributes.StrengthAttributeLevel++;
                break;
            case AttributeUpgradeTypes.UpgradeType.speed:
                attributes.SpeedAttributeLevel++;
                break;
            case AttributeUpgradeTypes.UpgradeType.dashAttack:
                attributes.DashAttack_AttributeLevel++;
                break;
            case AttributeUpgradeTypes.UpgradeType.dashSlam:
                attributes.DashSlam_AttributeLevel++;
                break;
            case AttributeUpgradeTypes.UpgradeType.postDashAttack:
                attributes.PostDashAttack_AttributeLevel++;
                break;

            case AttributeUpgradeTypes.UpgradeType.evade:
                attributes.EvadeAttributeLevel++;
                break;

            default:
                break;
        }

        SetAttributeValues();
    }

    bool IAttributeStoreCustomer.CanPurchaseUpgrade(int _price)
    {
        // check it if upgrade already purchased
        // and switch  out silver faith
        if (gameManager.TotalSilver < _price)
            return false;

        gameManager.TotalSilver -= _price;
        return true;
    }

    #endregion
}
   
