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

    private PlayerController _pc; // reference to main Player Controller
    private GameManager _gameManager; // reference to main Player Controller

    // left stick values
    [HideInInspector]
    public float L_xRaw;
    [HideInInspector]
    public float L_yRaw;
    private Vector2 _leftStickDirection;
    // right stick values
    [HideInInspector]
    public float R_xRaw;
    [HideInInspector]
    public float R_yRaw;

    //anything less than this value (.5f), joysticks become very "sensitive" along x axis
    [Tooltip("Custom Deadzone for Left Joystick")]
    public float LS_DeadZone = .75f;
    [Tooltip("Custom Deadzone for Right Joystick")]
    public float RS_DeadZone = 0.9f;

    // 
    // 
    [Tooltip(".22f is the desired value because it offers more grandularity but there's issues using " +
    "it. Assigned to 0f in the inspector for now.")]
    public float Aiming_DeadZone = .22f;

    [HideInInspector]
    public bool aiming = false;
    private Vector2 _rightStickAimDirection;
    // direction player is facing while aiming
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
    public float MAXEvadeScale = .3f;
    public float MAXDashDelay = .3f;
    public float MAXJumpDelay = .3f;

    //[HideInInspector]
    public bool evadeDelayComplete = true;
    [HideInInspector]
    public bool dashDelayComplete = true;
    [HideInInspector]
    public bool jumpDelayComplete = true;


    // On draw gizmo properties
    private float _curHitDistance = 10f;
    //private float _circleCastRadius = 1f;
    private Vector3 _origin;
    private Vector3 _boxCastSize;

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
            L_xRaw = Input.GetAxisRaw("Horizontal");
            L_yRaw = Input.GetAxisRaw("Vertical");
            _leftStickDirection = new Vector2(L_xRaw, L_yRaw);
            int lastDirection = _pc.dir;

            // if weapon is equipped
            if (_pc.EquippedWeapon)
            {
                // check if player is holding aim button/trigger.
                if (Input.GetButton("AimWeapon") && !aiming)
                    aiming = true;
                if (Input.GetButtonUp("AimWeapon"))
                {
                    aiming = false;
                    _pc.StateManager.EnterState(Entity.State.Idle);
                    _pc.dir = _aimDir;
                    // reset weapon position/rotation to default with movement direction
                    _pc.EquippedWeapon.GetComponent<Weapon>().ResetPosition(_aimDir);
                    // reset aim direction values
                    _aimDir = lastDirection = 0;

                }

                if (aiming)
                {
                    R_xRaw = Input.GetAxisRaw("RightStick_Horizontal");
                    R_yRaw = Input.GetAxisRaw("RightStick_Vertical");
                    _rightStickAimDirection = new Vector2(R_xRaw, R_yRaw);

                    // detect whether right stick is moved.
                    if (_rightStickAimDirection.magnitude < RS_DeadZone) // RS_DeadZone overrides unity's default input deadzone settings
                    {
                        _rightStickAimDirection = Vector2.zero;
                        if (_aimDir == 0)
                            _aimDir = _pc.dir;
                        else if (_aimDir != lastDirection)
                        {
                            _pc.FlipEntitySprite(_aimDir);
                            _pc.EquippedWeapon.GetComponent<Weapon>().FlipWeaponSprite(_aimDir);
                        }
                        _pc.EquippedWeapon.GetComponent<Weapon>().SetSpawnLoc();
                    }

                    else if (_rightStickAimDirection.magnitude > RS_DeadZone)
                    {
                        if (_rightStickAimDirection.x > Aiming_DeadZone)
                        {
                            Debug.Log("flip right");
                            _aimDir = 1;

                        }
                        else if (_rightStickAimDirection.x < -Aiming_DeadZone)
                        {
                            Debug.Log("flip left");
                            _aimDir = -1;
                        }
                        else
                        {
                            Debug.Log("neither");
                        }
                        Debug.Log($"_aimDir: {_aimDir}");
                        _pc.FlipEntityAimingSprite(_aimDir);
                        if (_aimDir != 0)
                        {
                            _pc.weaponManager.ModifyWeaponRotation(_aimDir, _rightStickAimDirection);
                            SwapAimingSprite(_aimDir, _rightStickAimDirection);
                        }
                        _pc.EquippedWeapon.GetComponent<Weapon>().FlipWeaponSprite(_aimDir);
                        _pc.EquippedWeapon.GetComponent<Weapon>().SetSpawnLoc();
                    }
                    lastDirection = _aimDir;
                }

                // Fire Weapon
                if (Input.GetButtonUp("Fire1") || Input.GetAxis("RightTrigger") == 0)
                    _pc.EquippedWeapon.GetComponent<Weapon>().ReleaseTrigger();

                if ((Input.GetButton("Fire1") || Input.GetAxis("RightTrigger") == 1))
                {
                    //Debug.Log("pressing trigger");
                    if (GetComponent<DashCommand>().dashState == DashCommand.DashState.completed)
                        Commands.Add(_fireCommand);
                }
            }

            if (_pc.CanMove)
            {
                // if no x-axis input registed, stop moving player  
                // *** maybe make an idle command and it's added to the command list instead ***
                if (_leftStickDirection.magnitude < LS_DeadZone)
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
                if (_leftStickDirection.magnitude > LS_DeadZone)
                {
                    if (_leftStickDirection.x > LS_DeadZone && _pc.dir != 1)
                        _pc.dir = 1;
                    else if (_leftStickDirection.x < -LS_DeadZone && _pc.dir != -1)
                        _pc.dir = -1;


                    // assign direction player is facing (maybe move this)
                    if (GetComponent<DashCommand>().dashState == DashCommand.DashState.completed)
                    {
                        _pc.FlipEntitySprite(_pc.dir);
                        // flip weapon sprite
                        if (_pc.weaponManager.WeaponEquipped && !aiming)
                            _pc.EquippedWeapon.GetComponent<Weapon>().FlipWeaponSprite(_pc.dir);
                        Commands.Add(_moveCommand);
                    }
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

    // aimDir is the values for an angle.
    // TODO: Rename this method. It's doing more than one thing. 
    // maybe replace
    private void SwapAimingSprite(int dir, Vector3 aimDir)
    {
        //float intervals = GetInterval based on stick position instead.
        var tmp = Quaternion.Euler(0, 0, GetTargetEuler(aimDir * dir, 45f));
        float roundedFloat = Mathf.Round(tmp.eulerAngles.z);

        Debug.Log($"dir: {dir} | aimDir: {aimDir}| roundedFloat:{roundedFloat}");
        Debug.Log($"tmp: {tmp}");
        switch (roundedFloat)
        {

            case 0:
                // center/facing-right: 0
                Debug.Log("case 0:");
                _pc.animator.Play("Player_Shoot");
                break;

            case 180:
                // center/facing-left: 0

                Debug.Log("case 180:");
                _pc.animator.Play("Player_Shoot");
                break;

            case 90:
                Debug.Log("case 90:");
                // this is weird. might have to change for 'dir' instead.
                //Debug.Log("angle.x < 0");
                // down/facing-left: 90
                if (dir == 1)
                    _pc.animator.Play("Player_Shoot_Up");
                else
                    _pc.animator.Play("Player_Shoot_Down");
                break;

            case 270:
                Debug.Log("case 270:");
                // down/facing-right: 270
                if (dir == 1)
                    _pc.animator.Play("Player_Shoot_Down");
                else
                    _pc.animator.Play("Player_Shoot_Up");
                break;

            case 45:
                Debug.Log("case 45:");
                //Debug.Log("angle.x > 0");
                if (dir == 1)
                    _pc.animator.Play("Player_Shoot_Angled_Up");
                else
                    _pc.animator.Play("Player_Shoot_Angled_Down");
                break;
            case 135:
                Debug.Log("case 135:");

                //_pc.animator.Play("Player_Shoot_Angled_Up");

                break;

            case 225:
                //Debug.Log("angle.x < 0");
                // down-angle/facing-left: 45
                //.9238796
                Debug.Log("case 225:");
                if (dir == 1)
                    _pc.animator.Play("Player_Shoot_Down");
                else
                    _pc.animator.Play("Player_Shoot_Up");

                break;

            case 315:
                Debug.Log("case 315:");
                if (dir == 1)
                    _pc.animator.Play("Player_Shoot_Angled_Down");
                else
                    _pc.animator.Play("Player_Shoot_Angled_Up");
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
}
