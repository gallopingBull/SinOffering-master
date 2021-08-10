using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    #region variables
    //command list
    private ICommand jumpCommand;
    private ICommand fireCommand;
    private ICommand moveCommand;
    private ICommand dashCommand;
    private ICommand evadeCommand;

    public List<ICommand> Commands;
    

    private PlayerController pc; //reference to main Player Controller

    public float x_DeadZone = .5f; //anything less than this value (.5f), joysticks become very "sensitive" along x axis


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


    #endregion


    #region functions
    // Use this for initialization
    void Awake()
    {
        pc = GetComponent<PlayerController>();

        jumpCommand = GetComponent<JumpCommand>();
        moveCommand = GetComponent<MoveCommand>();
        fireCommand = GetComponent<FireCommand>();
        dashCommand = GetComponent<DashCommand>();
        evadeCommand = GetComponent<EvadeCommand>();
    }

    public List<ICommand> HandleInput()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) ||
        Input.GetKeyDown("joystick button 7")))
        {
            GetComponent<Pause>().PauseGame();
        }

        if (pc.InputEnabled)
        {


            //Handles player's horizontal movement
            if (pc.xRaw > x_DeadZone ||
                pc.xRaw < -(x_DeadZone))
            {
                //assign direction player is facing (maybe move this)
                if (GetComponent<DashCommand>().dashState == DashCommand.DashState.completed)
                {
                    pc.FlipSprite();
                    Commands.Add(moveCommand);
                }
            }

            if (pc.AbilitiesEnabled)
            {
                //Handles Jumping
                if (Input.GetButtonDown("Jump") &&
                    pc.jumpEnabled &&
                    jumpDelayComplete)
                {
                    //*** check if this is necessarry **\\
                    //jumpDelay = MAXjumpDelay; 

                    pc.JumpButtonHeldDown = true;
                    #region TEST UI
                    if (pc.button != null &&
                        !pc.button.activeSelf)
                    {
                        pc.button.SetActive(true);
                    }
                    #endregion

                    if (GetComponent<DashCommand>().dashState ==
                        DashCommand.DashState.completed)
                    {
                        Commands.Add(jumpCommand);
                    }
                }

                if (Input.GetButtonUp("Jump"))
                {
                    pc.JumpButtonHeldDown = false;
                    if (pc.button != null &&
                        pc.button.activeSelf)
                    {
                        pc.button.SetActive(false);
                    }
                }


                //print("Lefytrigger"+Input.GetAxis("LeftTrigger"));
                //Dash Attack
                if (Input.GetAxis("LeftTrigger") == 1 &&
                    !GameManager.instance.gameCompleted)
                {
                    Commands.Add(dashCommand);
                    //print("InputHandler.css -> HandleInput() at: " + Time.realtimeSinceStartup);
                }

                //Evade/Dodge
                if ((Input.GetButtonDown("Fire2") ||
                    Input.GetKeyDown(KeyCode.LeftShift) ||
                    Input.GetKeyDown(KeyCode.RightShift)) && evadeDelayComplete)
                {
                    if (GetComponent<EvadeCommand>().FullyUpgraded)
                    {
                        if (GetComponent<EvadeCommand>().EvadeCount == 0)
                        {
                            evadeButtonPressedTime = Time.time;
                            //Debug.Log("evadeButtonPressedTime: " + evadeButtonPressedTime);
                            //Debug.Log("deactivate time: " + (evadeButtonPressedTime + MAXEvadeDelay));
                            evadeDelay = .3f; //find approaiate value to scale this down for when the player upgrades it
                                              // the delay is suppose ot be shorter
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
                    Commands.Add(evadeCommand);
                }

                //change weapons
                if (Input.GetButtonDown("ChangeWeapon"))
                {
                    pc.weaponManager.GetComponent<WeaponManager>().ChangeWeapon();
                }

                //change weapons -- delete later
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
                    moveCommand.Redo();
                }


                /**********
                 ***Fire***
                 **********/
                if (pc.EquippedWeapon != null)
                {
                    if (Input.GetButtonUp("Fire1") ||
                        Input.GetAxis("RightTrigger") == 0)
                        pc.EquippedWeapon.GetComponent<Weapon>().ReleaseTrigger();

                    //fire weapon
                    if ((Input.GetButton("Fire1") ||
                        Input.GetAxis("RightTrigger") == 1) &&
                        pc.EquippedWeapon != null)
                    {
                        print("pressing trigger");
                        if (GetComponent<DashCommand>().dashState == DashCommand.DashState.completed)
                            Commands.Add(fireCommand);
                    }
                }
                /**********
                ***--*END*--Fire--*END*--***
                **********/


            }






            //if no input, stop moving player (maybe make an idle command and it's added to the command list instead)
            if (pc.xRaw == 0)
            {
                if (!GetComponent<PlayerController>().animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle") &&
                    !GetComponent<PlayerController>().animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Shoot"))
                {
                    if (GetComponent<PlayerController>().IsGrounded)
                    {
                        //change to state to Player_Idle
                        if (!GetComponent<PlayerController>().EquippedWeapon)
                        {
                            GetComponent<PlayerController>().animator.Play("Player_Idle");
                        }
                        else
                        {
                            GetComponent<PlayerController>().animator.Play("Player_Shoot");
                        }
                        GetComponent<PlayerController>().state = Entity.State.Idle;
                    }
                }
                GetComponent<PlayerController>().rb.velocity = new Vector3(0, GetComponent<PlayerController>().rb.velocity.y, 0);
            }

         
            return Commands;
        }
        return null;
    }


    public void InputDelay()
    {
        switch (pc.state)
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

                //reset evade counter if evade button
                //not pressed again immediately for a double evade.
                //-- Condition should only be checked if player has fully upgraded
                //evade ability...
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
}
