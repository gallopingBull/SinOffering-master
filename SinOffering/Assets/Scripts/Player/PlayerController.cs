using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Entity, IWeaponStoreCustomer, IAttributeStoreCustomer
{
    #region variables

    public float Mana = 0;
    public float Strength = 0;

    //private bool _weaponsEnabled = true;
    //private bool _dashAbilityEnabled = true;
    //private bool _meleeEnabled = true;

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
    [SerializeField]
    private List<ICommand> commands;

    public float yRaw;

    [SerializeField]
    private PlayerAttributes _attributes = new PlayerAttributes();

    [HideInInspector]
    public bool AbilitiesEnabled = true; // turn off when player in areas that they can't use special abilities

    [HideInInspector]
    public bool CanDoubleJump = false;
    [HideInInspector]
    public bool JumpButtonHeldDown = false;

    //[HideInInspector]
    public float delay = 0f; // delay between jump input        
    //[HideInInspector]
    public const float MAX_DELAY = .3f; // delay between jump input

    //weapons variables
    public SpriteRenderer MeleeSprite;
    public SpriteRenderer BloodMeleeSprite;
    [HideInInspector]
    public GameObject EquippedWeapon;
    [HideInInspector]
    public WeaponManager weaponManager;

    private ParticleSystem _particleSystem;


    public AudioClip jumpClip, landClip, dashClip;
    //[HideInInspector]
    public GameObject button;

    [HideInInspector]
    public StateManager StateManager;
    [HideInInspector]
    public InputDelay InputDelay;



    public PlayerAttributes Attributes { get => _attributes; set => _attributes = value; }


    #endregion

    #region functions

    private void Awake() { instance = this; }

    // ***USE ONLY FOR PHYSICS CALCULATIONS*** \\\
    protected override void FixedUpdate()
    {
        inputHandler.InputDelay();
        
        #region testing
        //InputDelay2();
        //InputDelay.InputDelayHandler(state); // manages delay timers for several different input/actions
        #endregion
        CheckIfFalling();
        GravityModifier();
        CheckFloor();
    }

    // ***USE ONLY FOR INPUT*** \\\
    private void Update()
    {
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

    protected override void InitEntity()
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
        _particleSystem = GetComponent<ParticleSystem>(); 
        InputDelay = GetComponent<InputDelay>();
    }
    /*
    public override void Damaged(float damageValue)
    {
        base.Damaged(damageValue);

    }*/

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

        gameManager.IncrementDeathCount();
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

    protected override void CheckIfFalling()
    {
        //Debug.Log("rb.velocity.y: " + rb.velocity.y);
        if (GetComponent<DashCommand>().dashState != DashCommand.DashState.completed || state == State.evading)
            return;
        if (!IsGrounded)
        {
            if (rb.velocity.y > 0)
            {
                // maybe have this should be set in the statemanager
                if (state != Entity.State.Jumping)
                    StateManager.EnterState(State.Jumping);
            }
            if (rb.velocity.y < 0)
            {
                if (state != State.falling)
                    StateManager.EnterState(State.falling);
            }
        }
    }   

    public void EnableInput()
    {
        InputEnabled = true;
    }

    public void DisableInput()
    {
        InputEnabled = false;
    }

    public void SetPersistentPlayerAttributeData(PlayerAttributes _attributes)
    {
        this._attributes = _attributes;
        SetAttributeValues();
    }
     
    private void SetAttributeValues()
    {
        var dataBase = AttributeDatabase._instance.GetAttributeDatabase();

        int maxLength = dataBase["health"].AttributeDataList.Length;
        if (_attributes.HealthAttributeLevel < maxLength)
            Health = dataBase["health"].AttributeDataList[_attributes.HealthAttributeLevel].AttributeValue;
        
        maxLength = dataBase["speed"].AttributeDataList.Length;
        if (_attributes.SpeedAttributeLevel < maxLength)
            Speed = dataBase["speed"].AttributeDataList[_attributes.SpeedAttributeLevel].AttributeValue;

        maxLength = dataBase["mana"].AttributeDataList.Length;
        if (_attributes.ManaAttributeLevel < maxLength)
            Mana = dataBase["mana"].AttributeDataList[_attributes.ManaAttributeLevel].AttributeValue;

        maxLength = dataBase["strength"].AttributeDataList.Length;
        if (_attributes.StrengthAttributeLevel < maxLength)
            Strength = dataBase["strength"].AttributeDataList[_attributes.StrengthAttributeLevel].AttributeValue;
    }

    // store functions (might want to move these interfaces in to their own class,
    // and give playercontroller.cs its own instance of that cla ss)
    #region IWeaponStoreCustomer, IAttributeStoreCustomer

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
        SetAttributeValues();

        //Debug.Log("should purchase: " + _upgradeType.ToString());
        var dataBase = AttributeDatabase._instance.GetAttributeDatabase();

        // look into this for negative value
        int maxLevel = dataBase[_upgradeType.ToString()].AttributeDataList.Length;

        //Debug.Log("maxLevel: " + maxLevel + " || playerAttributeLevels" + dataBase[_upgradeType.ToString()]);
        switch (_upgradeType)
        {
            case AttributeUpgradeTypes.UpgradeType.health:
                _attributes.HealthAttributeLevel++;
                break;
            case AttributeUpgradeTypes.UpgradeType.mana:
                _attributes.ManaAttributeLevel++;
                break;
            case AttributeUpgradeTypes.UpgradeType.strength:
                _attributes.StrengthAttributeLevel++;
                break;
            case AttributeUpgradeTypes.UpgradeType.speed:
                _attributes.SpeedAttributeLevel++;
                break;
            case AttributeUpgradeTypes.UpgradeType.dashAttack:
                _attributes.DashAttack_AttributeLevel++;
                break;
            case AttributeUpgradeTypes.UpgradeType.dashSlam:
                _attributes.DashSlam_AttributeLevel++;
                break;
            case AttributeUpgradeTypes.UpgradeType.postDashAttack:
                _attributes.PostDashAttack_AttributeLevel++;
                break;

            case AttributeUpgradeTypes.UpgradeType.evade:
                _attributes.EvadeAttributeLevel++;
                break;

            default:
                break;
        }
        //SaveSystem._instance.SaveGameData();
    }

    bool IAttributeStoreCustomer.CanPurchaseUpgrade(int _price)
    {
        if (gameManager.TotalCurrentFaith < _price)
            return false;

        gameManager.TotalCurrentFaith -= _price;
        return true;
    }

    #endregion

    #endregion
}
   
 