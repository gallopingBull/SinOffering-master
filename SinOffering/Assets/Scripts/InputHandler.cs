using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    #region variables
    //command list
    private Command _jumpCommand;
    private Command _fireCommand;
    private Command _moveCommand;
    private Command _dashCommand;
    private Command _evadeCommand;
    private Command _meleeCommand;

    [SerializeField]
    public List<Command> Commands;
    
    private PlayerController _pc; //reference to main Player Controller
    private GameManager _gameManager; //reference to main Player Controller

    // left stick values
    [HideInInspector]
    public float L_xRaw;
    [HideInInspector]
    public float L_yRaw;
    // right stick values
    [HideInInspector]
    public float R_xRaw;
    [HideInInspector]
    public float R_yRaw;

    //anything less than this value (.5f), joysticks become very "sensitive" along x axis
    public const float x_DeadZone = .5f; 

    [HideInInspector]
    public bool aiming = false;
    private Vector3 _aimDirection;
    private int movementDir = 0;
    [HideInInspector]
    public int _aimDir = 0;

    [Header("Input Delay Variables")]
    //[HideInInspector]
    public float evadeDelay = 0f;
    [HideInInspector]
    public float dashDelay = 0f;
    [HideInInspector]
    public float jumpDelay = 0f;

    private float evadeButtonPressedTime = 0;

    public float MAXEvadeDelay = .3f;
    [Tooltip("only used when a quick double evade is unlocked")]
    public float MAXEvadeScale= .3f;     
    public float MAXDashDelay = .3f;
    public float MAXjumpDelay = .3f;

    //[HideInInspector]
    public bool evadeDelayComplete = true;
    [HideInInspector]
    public bool dashDelayComplete = true;
    [HideInInspector]
    public bool jumpDelayComplete = true;



    // On draw gizmo properties
    private float _curHitDistance = 10f;
    private float _circleCastRadius = 1f;
    private Vector3 _origin;
    private Vector3 _boxCastSize;

    public float deadZone = 0.05f;
   
    #endregion

    #region functions
    void Awake()
    {
        _pc = GetComponent<PlayerController>();
        _gameManager = GameManager.Instance;
        _jumpCommand = GetComponent<JumpCommand>();
        _moveCommand = GetComponent<MoveCommand>();
        _fireCommand = GetComponent<FireCommand>();
        _dashCommand = GetComponent<DashCommand>();
        _evadeCommand = GetComponent<EvadeCommand>();
        _meleeCommand = GetComponent<MeleeCommand>();
        _boxCastSize = new Vector3(1, 1, 1);
    }

    public List<Command> HandleInput()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 7")))
            GetComponent<Pause>().PauseGame();

        if (_pc.InputEnabled)
        {
            #region get move direction
            L_xRaw = Input.GetAxisRaw("Horizontal");
            L_yRaw = Input.GetAxisRaw("Vertical");

            #region testing
            //InputDelay2();
            //InputDelay.InputDelayHandler(state); // manages delay timers for several different input/actions
            #endregion

            int lastDirection = _pc.dir;
            if (L_xRaw > 0 && _pc.dir != 1)
                _pc.dir = 1;
            if (L_xRaw < 0 && _pc.dir != -1)
                _pc.dir = -1;

            #endregion

            // if weapon is equipped
            if (_pc.weaponManager.WeaponEquipped)
            {
                if (Input.GetButton("AimWeapon") && !aiming)
                    aiming = true;
                if (Input.GetButtonUp("AimWeapon"))
                {
                    aiming = false;
                    _pc.StateManager.EnterState(Entity.State.Idle);
                    // reset weapon position/rotation to default with movement direction
                    _pc.EquippedWeapon.GetComponent<Weapon>().ResetPosition(_aimDir);
                }

                if (aiming)
                {
                    R_xRaw = Input.GetAxisRaw("RightStick_Horizontal");
                    R_yRaw = Input.GetAxisRaw("RightStick_Vertical");
                    _aimDirection = new Vector2(R_xRaw, R_yRaw);

                    #region testing
                    //Debug.Log("_aimDirection.magnitude" + _aimDirection.magnitude);
                    if (_aimDirection.magnitude < deadZone)
                    {
                        //_aimDirection = Vector2.zero;
                    }
                    #endregion

                    if (R_xRaw != 0)
                        _aimDir = R_xRaw > 0 ? 1 : -1;
                   
                    if (R_xRaw == 0)
                    {
                        if (_aimDir == 0)
                            _aimDir = _pc.dir;
                        else
                        {
                            if (lastDirection != _aimDir)
                            {
                                _pc.FlipEntitySprite(_aimDir);
                                _pc.EquippedWeapon.GetComponent<Weapon>().FlipWeaponSprite(_aimDir);
                            }
                        }
                    }
                    else
                    {
                        _pc.FlipEntityAimingSprite(_aimDir);
                        if (_aimDir != 0) 
                        { 
                            _pc.weaponManager.ModifyWeaponRotation(_aimDir, _aimDirection); 
                            SwapAimingSprite(_aimDir, _aimDirection);
                        }
                        _pc.EquippedWeapon.GetComponent<Weapon>().FlipWeaponSprite(_aimDir);
                    }
                    _pc.EquippedWeapon.GetComponent<Weapon>().SetSpawnLoc();
                    lastDirection = _aimDir;
                }
                else
                    _aimDir = lastDirection = 0;
            }

            // if no x-axis input registed, stop moving player
            // *** maybe make an idle command and it's added to the command list instead ***
            if (L_xRaw == 0)
            {
                // maybe create a "IdleCommand" and move this there... idk yet
                if (_pc.IsGrounded && !aiming)
                {
                    if (_pc.state != Entity.State.Idle && !Input.GetButtonDown("Jump"))
                        _pc.StateManager.EnterState(Entity.State.Idle);
                }
                _pc.rb.velocity = new Vector3(0, _pc.rb.velocity.y, 0);
            }
    
            // Handles player's horizontal movement
            if (L_xRaw > x_DeadZone || L_xRaw < -(x_DeadZone))
            {
                // assign direction player is facing (maybe move this)
                if (GetComponent<DashCommand>().dashState == DashCommand.DashState.completed)
                {
                    _pc.FlipEntitySprite(_pc.dir);
                    // flip weapon sprite
                    if (_pc.weaponManager.WeaponEquipped)
                        if (!aiming)
                            _pc.EquippedWeapon.GetComponent<Weapon>().FlipWeaponSprite(_pc.dir);
                    Commands.Add(_moveCommand);
                }   
            }

            if (_pc.AbilitiesEnabled)
            {
                if (Input.GetButtonUp("Jump"))
                {
                    if (_pc.button != null && _pc.button.activeSelf)
                        _pc.button.SetActive(false);
                }
                // Handles Jumping
                if (Input.GetButtonDown("Jump"))
                {
                    //Debug.Log("GetButtonDown ||pc.jumpcount = " + pc.jumpCount + " -------");

                    #region testing input delay
                    //*** check if this is necessarry **\\
                    //jumpDelay = MAXjumpDelay; 
                    #endregion

                    #region TEST UI
                    if (_pc.button != null && !_pc.button.activeSelf)
                        _pc.button.SetActive(true);
                    #endregion
                    
                    if (_pc.jumpEnabled && jumpDelayComplete)
                    {
                        if (GetComponent<DashCommand>().dashState == DashCommand.DashState.completed)
                            Commands.Add(_jumpCommand);
                    }
                }

                // Dash Attack
                if (Input.GetAxis("LeftTrigger") == 1 && !_gameManager.GameCompleted)
                    Commands.Add(_dashCommand);

                // Melee Attack
                if (Input.GetButtonDown("Melee") && !_gameManager.GameCompleted)
                    Commands.Add(_meleeCommand);


                // Evade/Dodge
                if ((Input.GetButtonDown("Fire2") ||
                    Input.GetKeyDown(KeyCode.LeftShift) ||
                    Input.GetKeyDown(KeyCode.RightShift)) && evadeDelayComplete)
                {
                    if (GetComponent<EvadeCommand>().FullyUpgraded)
                    {
                        if (GetComponent<EvadeCommand>().EvadeCount == 0)
                        {
                            evadeButtonPressedTime = Time.time;
                            #region test logs
                            //Debug.Log("evadeButtonPressedTime: " + evadeButtonPressedTime);
                            //Debug.Log("deactivate time: " + (evadeButtonPressedTime + MAXEvadeDelay));
                            #endregion

                            // find  approaiate value to scale this down for when the player upgrades it
                            // the delay is suppose ot be shorter
                            evadeDelay = .3f;           
                            //Debug.Log("evadeDelay: "+ evadeDelay);
                        }
                        else
                        {
                            evadeDelay = MAXEvadeDelay;
                            //Debug.Log(" more than oneevadeDelay: " + evadeDelay);
                        }
                    }
                    else
                    {
                        if (GetComponent<EvadeCommand>().EvadeCount == 0)
                        {
                            evadeButtonPressedTime = Time.time;
                            //Debug.Log("evadeButtonPressedTime: " + evadeButtonPressedTime);
                            //Debug.Log("deactivate time: " + (evadeButtonPressedTime + MAXEvadeDelay));
                        }
                        evadeDelay = MAXEvadeDelay;
                        //Debug.Log("evadeDelay: " + evadeDelay);
                    }
                    Commands.Add(_evadeCommand);
                }

                // change weapons
                if (Input.GetButtonDown("SwapWeapon"))
                {
                    _pc.weaponManager.ChangeWeapon();
                    _pc.StateManager.EnterState(_pc.state);
                }
                    

                #region select weapons with numeric keys -- delete later
                if (Input.GetKey(KeyCode.Alpha1))
                {
                    GetComponent<PlayerController>().weaponManager.GetComponent<WeaponManager>().EquipWeapon(0);
                }
                if (Input.GetKey(KeyCode.Alpha2))
                {
                    GetComponent<PlayerController>().weaponManager.GetComponent<WeaponManager>().EquipWeapon(1);
                }
                if (Input.GetKey(KeyCode.Alpha3))
                {
                    GetComponent<PlayerController>().weaponManager.GetComponent<WeaponManager>().EquipWeapon(2);
                }
                if (Input.GetKey(KeyCode.Alpha4))
                {
                    GetComponent<PlayerController>().weaponManager.GetComponent<WeaponManager>().EquipWeapon(3);
                }
                if (Input.GetKey(KeyCode.Alpha5))
                {
                    print("rewind time");
                    _moveCommand.Redo();
                }

                #endregion
                
                // Fire Weapon
                if (_pc.EquippedWeapon != null)
                {
                    if (Input.GetButtonUp("Fire1") || Input.GetAxis("RightTrigger") == 0)
                        _pc.EquippedWeapon.GetComponent<Weapon>().ReleaseTrigger();

                    if ((Input.GetButton("Fire1") || Input.GetAxis("RightTrigger") == 1) &&
                        _pc.EquippedWeapon != null)
                    {
                        //Debug.Log("pressing trigger");
                        if (GetComponent<DashCommand>().dashState == DashCommand.DashState.completed)
                            Commands.Add(_fireCommand);
                    }   
                }
            }
            return Commands;
        }
        return null;
    }

    public void InputDelay()
    {
        switch (_pc.state)
        {
            case Entity.State.Jumping:
                if (jumpDelay > 0f)
                {
                    jumpDelayComplete = false;
                    jumpDelay -= Time.fixedDeltaTime;
                    //if (InputDelay.jumpDelayComplete)
                }
                if (jumpDelay < 0f)
                {
                    jumpDelayComplete = true;
                    jumpDelay = 0;
                    //if (!InputDelay.jumpDelayComplete)     
                }
                break;
            case Entity.State.evading:

                if (evadeDelay > 0f)
                {
                    //if (evadeDelayComplete)

                    evadeDelayComplete = false;
                    evadeDelay -= Time.deltaTime;
                }
                if (evadeDelay < 0f)
                {
                    //if (!evadeDelayComplete)
                    evadeDelayComplete = true;
                    evadeDelay = 0;
                }

                //reset evade counter if evade button
                //not pressed again immediately for a double evade.
                //-- Condition should only be checked if player has fully upgraded
                //evade ability...
                if (Time.time > evadeButtonPressedTime + MAXEvadeDelay &&
                    GetComponent<EvadeCommand>().EvadeCount == 1 && 
                    evadeDelayComplete)
                {
                    GetComponent<EvadeCommand>().EvadeCount = 0;
                    GetComponent<EvadeCommand>().AirEvadeCount = 0;
                }
                break;

            case Entity.State.dashing:
                if (dashDelay > 0)
                {
                    if (dashDelayComplete)
                        dashDelayComplete = false;
                    dashDelay -= Time.deltaTime;
                }
                if (dashDelay < 0)
                {
                    if (!dashDelayComplete)
                        dashDelayComplete = true;
                    dashDelay = 0;
                }
                break;
            case Entity.State.falling:
            case Entity.State.Idle:
            case Entity.State.running:
                if (jumpDelay > 0)
                {
                    jumpDelayComplete = false;
                    jumpDelay -= Time.fixedDeltaTime;
                    //if (InputDelay.jumpDelayComplete);
                }
                if (jumpDelay < 0f)
                {
                    jumpDelayComplete = true;
                    jumpDelay = 0;
                    //if (!InputDelay.jumpDelayComplete)     
                }

                if (evadeDelay > 0f)
                {
                    //if (evadeDelayComplete)

                    evadeDelayComplete = false;
                    evadeDelay -= Time.deltaTime;
                }
                if (evadeDelay < 0f)
                {
                    //if (!evadeDelayComplete)
                    evadeDelayComplete = true;
                    evadeDelay = 0;
                  
                }

                // reset evade counter if evade button
                // not pressed again immediately for a double evade. -- Condition should only be checked if player has fully upgraded
                // evade ability...
                if (Time.time > evadeButtonPressedTime + MAXEvadeDelay &&
                      GetComponent<EvadeCommand>().EvadeCount == 1 && evadeDelayComplete)
                {
                    print("resetting evade counter, button not pressed twice");
                    GetComponent<EvadeCommand>().EvadeCount = 0;
                    GetComponent<EvadeCommand>().AirEvadeCount = 0;
                }

                if (dashDelay > 0)
                {
                    //if (dashDelayComplete)
                    dashDelayComplete = false;
                    dashDelay -= Time.deltaTime;
                }
                if (dashDelay < 0)
                {
                    //if (!dashDelayComplete)
                    dashDelayComplete = true;
                    dashDelay = 0;
                }
                break;


            default:
                break;
                
        }

    }
    #endregion

    private void OnDrawGizmos()
    {
        // crappy condition to ignore null error from pc while in editor
        if (_pc == null)
            return;

        if (_pc.EquippedWeapon != null)
        {
            _origin = _pc.EquippedWeapon.GetComponent<Weapon>().spawnLoc.transform.position;

            // aim direction
            if (aiming && _origin != null)
            {
                Quaternion currentRotation = _pc.EquippedWeapon.GetComponent<Weapon>().spawnLoc.transform.localRotation;
                Vector3 currentEulerAngles = currentRotation * Vector3.right;

                Gizmos.color = Color.red;
                Debug.DrawRay(_origin, currentEulerAngles, Color.red);
                Gizmos.DrawWireCube(_origin + (currentEulerAngles * _curHitDistance), _boxCastSize);
                //Gizmos.DrawWireSphere(origin + (direction.normalized * curHitDistance), circleCastDashRadius);
            }
        }


        /*
        // move direction
        Gizmos.color = Color.blue;

        Debug.DrawRay(_origin, _aimDirection.normalized * _curHitDistance, Color.green);
        Gizmos.DrawWireCube(_origin + (_aimDirection.normalized * _curHitDistance), _boxCastSize);
         */
    }

    private void SwapAimingSprite(int dir, Vector3 angle)
    {
        var tmp = Quaternion.Euler(0, 0, GetTargetEuler(angle * dir, 45f));
        float roundedFloat = Mathf.Round(tmp.eulerAngles.z);

        switch (roundedFloat)
        {
            case 45:
                //Debug.Log("case 45:");
                if (angle.x > 0)
                {
                    Debug.Log("angle.x > 0");
                    _pc.animator.Play("Player_Shoot_Angled_Up")  ;
                }
                else
                {
                    //Debug.Log("angle.x < 0");
                    // down-angle/facing-left: 45
                    //.9238796
                    _pc.animator.Play("Player_Shoot_Angled_Down");
                    //if (!pc.animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Shoot_Angled_Down"))
                }
                break;
           
            
            case 0:
                // center/facing-left: 0
                // center/facing-right: 0


                //Debug.Log("case 0:");
                _pc.animator.Play("Player_Shoot");
                break;

            case 270:
                //Debug.Log("case 270:");
                if (angle.x > 0)
                {
                    //Debug.Log("angle.x > 0");
                    // down/facing-right: 270
                    if (!_pc.animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Shoot_Down"))
                        _pc.animator.Play("Player_Shoot_Down");
                }
                else
                {
                    //Debug.Log("angle.x < 0");
                    // up/facing-left: 270  
                    if (!_pc.animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Shoot_Up"))
                        _pc.animator.Play("Player_Shoot_Up");
                }
                break;

            case 315:
                //Debug.Log("case 315:");
                if (angle.x > 0)
                {
                    //Debug.Log("angle.x > 0");
                    // down-angle/facing-right: 315
                    if (!_pc.animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Shoot_Angled_Down"))
                        _pc.animator.Play("Player_Shoot_Angled_Down");
                }
                else
                {
                    //Debug.Log("angle.x < 0");
                    // up-angle/facing-left: 315
                    if (!_pc.animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Shoot_Angled_Up"))
                        _pc.animator.Play("Player_Shoot_Angled_Up");
                }
                break;


            case 90:
                //Debug.Log("case 90:");
                if (angle.x > 0)
                {
                    //Debug.Log("angle.x > 0");
                    // up/facing-right: 90
                    if (!_pc.animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Shoot_Down"))
                        _pc.animator.Play("Player_Shoot_Down");
                }
                else
                {
                    //Debug.Log("angle.x < 0");
                    // down/facing-left: 90
                    if (!_pc.animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Shoot_Up"))
                        _pc.animator.Play("Player_Shoot_Up");
                }

                break; 

            default:
                break;
        }
    }
        
    public float GetTargetEuler(Vector3 touchPosition, float interval)
    {
        float currentAngle = Mathf.Atan2(touchPosition.y, touchPosition.x) * Mathf.Rad2Deg;
        currentAngle = (currentAngle > 0) ? currentAngle : currentAngle + 360f;

        var region = (int)Mathf.Floor(currentAngle / interval);

        return region * interval;
    }
    
    public void ModifyWeaponRotation(int dir, Vector3 angle)
    {
        _pc.EquippedWeapon.transform.rotation =
            Quaternion.Euler(0, 0, GetTargetEuler(angle * dir, 45f));
    }
}
