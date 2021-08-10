using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour {
    #region variables
    public float life = 3;

    //Movement Variables
    public float Speed = 0;
    public float MaxSpeed = 10;
    public float AccSpeed = .15f;
    public float DeAccSpeed = .15f;

    
    public float JumpSpeed = 10;
    [HideInInspector]
    public float defaultRunSpeed;  //never change these values after being assigned;
    [HideInInspector]
    public float defaultJumpSpeed; //never change these values after being assigned;
    [HideInInspector]
    public int jumpCount = 0;

    public float FallMultiplier = 2.5f;
    public float LowJumpMultiplier = 2f;
    public float MAX_Y_Vel = 3;

    [HideInInspector]
    public Rigidbody rb;

    [HideInInspector]
    public enum State { Idle, Jumping, falling, firing, running, dashing, evading };
    //[HideInInspector]
    public bool IsGrounded = false;

    //[HideInInspector]
    public State state; // Actor's current state

    public bool isInvincible;

    public SpriteRenderer ActorSprite;
    public SpriteRenderer BloodActorSprite;
    
    
    [HideInInspector]
    public bool jumpEnabled = true;


    [HideInInspector]
    public bool facingLeft = false; 

    public int dir = 1; //direction actor sprite is facing (1 = right, -1 = left)
    
    //Linecast variables 
    public LayerMask LayerMask;

    [SerializeField]
    protected Transform GroundCheck, GroundCheckL, GroundCheckR;

    protected float xPos, yPos; //actor's position for the next frame

    protected GameManager gm;
    protected CameraManager camManager;
    public List<Collider> colliders;



    //foot dust trail variables
    [SerializeField]
    protected bool EnableDustTrails = false; //field for user to toggle dust trails from inspector 
    public GameObject[] Meshes_DustTrails;
    protected bool canSpawnDustTrail;
    protected int dustTrailIndex = 0;

    protected int curSteps;


    public float AmmountToGround = .75f;
    [SerializeField]
    protected float stepRate = .08f;
    [SerializeField]
    protected float MaxStepRate = .08f;


    //blood splat variables 
    public int MaxBloodMasks = 12;
    //[HideInInspector]
    public int BloodCount = 0;

    #endregion

    #region functions
    protected abstract void FixedUpdate();
    protected abstract void InitActor();

    //Called when actor has been killed.
    public abstract void Killed();


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gm = GameManager.instance;
        camManager = CameraManager.instance; 

        InitActor();

        //Invoke("DeParentCaller", 4f);
    }

    protected void CheckIfFalling()
    {
        if (rb.velocity.y > 0)
        {
            //maybe have this be set in the statemanager
            if (state != Entity.State.Jumping)
                state = State.Jumping;
        }
        if (rb.velocity.y < 0)
        {
            if (state != State.falling)
                state = State.falling;
        }
        else if (rb.velocity.y == 0 && IsGrounded) /// &&isGrounded???
        {
            if (state != State.Idle)
                state = State.Idle;
        }
        else { }
    }

    protected void GravityModifier()
    {
        //check if player is jumping, and then set to jumping state, 
        if (state == Entity.State.Jumping)
        {
            //if jump button is released before reaching max jump height, apply more 
            //gravity so player falls faster.
            //NOTE**maybe move this condition into input handler**\\
            if (Input.GetButtonUp("Jump"))
            {
                //LowJumpMultipllier value should remain higer than FallMulitplier 
                //so player falls faster for shorter 
                rb.velocity += Vector3.up * Physics.gravity.y * (LowJumpMultiplier - 1) * Time.fixedDeltaTime * TimeScale.player;
            }
        }

        //if actor has reached max velocity/y position
        //the player will start falling as way to clamp the player's jump height
        if (state == Entity.State.Jumping && rb.velocity.y > MAX_Y_Vel)
        {
            //this basically checks if there in dash mode(about to dash)
            if (state != State.dashing && rb.useGravity)
                rb.velocity += Vector3.up * Physics.gravity.y * (10 - 1) * Time.fixedDeltaTime * TimeScale.player;
        }

        if (state == Entity.State.falling)
        {
            if (name == "Player")
            {
                //this basically checks if there in dash mode(about to dash)
                if (state != State.dashing && rb.useGravity)
                    rb.velocity += Vector3.up * Physics.gravity.y * (FallMultiplier - 1) * Time.fixedDeltaTime * TimeScale.player;
            }
            //this else statement is redundant, check why it's here
            else
            {
                //print("adjusting velocity and state == falling for enemy");
                rb.velocity += Vector3.up * Physics.gravity.y * (FallMultiplier - 1) * Time.fixedDeltaTime * TimeScale.enemies;
                if (state != State.falling)
                    state = State.falling;
            }
        }
    }

    //redundant if conditions, consolidate this into a for loop
    protected void CheckFloor()
    {
        if (Physics.Linecast(transform.position, GroundCheckR.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            if (!IsGrounded)
            {
                if (name == "Player")
                {
                    //GetComponent<PlayerController>().animator.Play("Player_Idle");
                    if (!Input.GetButton("Jump") && GetComponent<PlayerController>().jumpEnabled)
                    {
                        if (state != State.dashing)
                        {
                            IsGrounded = true;
                        }
                    }

                    SoundManager.PlaySound(GetComponent<PlayerController>().landClip);
                    jumpCount = 0;
                }
                else
                {
                    IsGrounded = true;
                }
                jumpEnabled = true;
            }
        }
        else if (Physics.Linecast(transform.position, GroundCheck.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            if (!IsGrounded)
            {
                if (name == "Player")
                {
                    //GetComponent<PlayerController>().animator.Play("Player_Idle");
                    if (!Input.GetButton("Jump") && GetComponent<PlayerController>().jumpEnabled)
                    {
                        if (state != State.dashing)
                        {
                            IsGrounded = true;
                        }
                    }

                    SoundManager.PlaySound(GetComponent<PlayerController>().landClip);
                    jumpCount = 0;
                }
                else
                {
                    IsGrounded = true;
                }

                jumpEnabled = true;
            }
        }
        else if (Physics.Linecast(transform.position, GroundCheckL.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            if (!IsGrounded)
            {
                if (name == "Player")
                {
                    //GetComponent<PlayerController>().animator.Play("Player_Idle");
                    if (!Input.GetButton("Jump") && GetComponent<PlayerController>().jumpEnabled)
                    {
                        if (state != State.dashing)
                        {
                            IsGrounded = true;
                        }
                    }

                    SoundManager.PlaySound(GetComponent<PlayerController>().landClip);
                    jumpCount = 0;
                }
                else
                {
                    IsGrounded = true;
                }
                jumpEnabled = true;
            }
        }
        else
        {
            IsGrounded = false;
        }
    }

    //Called when actor has taken damaged.
    public void Damage(float _damageValue)
    {
        if (!isInvincible)
        {
            StartCoroutine("DamageIndicator");

            if (life <= 1)
            {
                Killed();
            }
            else { life -= _damageValue; }
        }
    }

    private IEnumerator DamageIndicator()
    {
        if (ActorSprite != null) {
            ActorSprite.color = Color.red;
            yield return new WaitForSeconds(.05f);
            ActorSprite.color = Color.white;
            StopCoroutine("DamageIndicator");
        }
    }

    //This is called to remove all blood masks attached
    //to this entity when entitiy is destoryed.
    protected void DeParentCaller()
    {
        Transform[] transforms;
        transforms = GetComponentsInChildren<Transform>();

        foreach (Transform mask in transforms)
        {
            if (mask.name == "Blood Sprite Mask(Clone)")
            {
                print("removing mask from" + gameObject.name);
                MaskDecalPool.instance.DeParentMasks(mask.gameObject);
            }
                
        }
        print("finshed removing masks from: " + gameObject.name);
    }

    //flip enity's sprite
    //make this so it flips any sprite put into it
    public void FlipSprite()
    {
        //assign direction player is facing
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            if (dir != 1)
            {
                //GetComponent<PlayerController>().dir = GetComponent<Entity>().xPos > 0 ? 1 : -1;
                dir = 1;
                facingLeft = false;
                ActorSprite.GetComponent<SpriteRenderer>().flipX = GetComponent<Entity>().facingLeft;
                BloodActorSprite.GetComponent<SpriteRenderer>().flipX = GetComponent<Entity>().facingLeft;

                //flip weapon sprite
                if (GetComponent<PlayerController>().EquippedWeapon != null)
                {
                    GetComponent<PlayerController>().EquippedWeapon.GetComponent<Weapon>().FlipWeapon(GetComponent<PlayerController>().dir);
                }
            }
        }
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            if (dir != -1)
            {
                dir = -1;
                facingLeft = true;
                ActorSprite.GetComponent<SpriteRenderer>().flipX = GetComponent<Entity>().facingLeft;
                BloodActorSprite.GetComponent<SpriteRenderer>().flipX = GetComponent<Entity>().facingLeft;

                //flip weapon sprite
                if (GetComponent<PlayerController>().EquippedWeapon != null)
                {
                    GetComponent<PlayerController>().EquippedWeapon.GetComponent<Weapon>().FlipWeapon(GetComponent<PlayerController>().dir);
                }
            }
        }
        else
        {
            
        }
    }

    //Set default values for player parameters
    protected void SetDefaultSpeed()
    {
        defaultRunSpeed = Speed;
        defaultJumpSpeed = JumpSpeed;
    }
    
    public void ResetSpeed()
    {
        JumpSpeed = defaultJumpSpeed;
        Speed = defaultRunSpeed;
    }

    protected void SpawnDustTrail()
    {
        //print(gameObject.name + " spawming dust trail");
        if (dustTrailIndex <= 2)
        {
            GameObject tmp; 
            tmp = Instantiate(Meshes_DustTrails[dustTrailIndex], transform.position, transform.rotation);
            tmp.GetComponent<DustTrail>().actor = gameObject;
            tmp.GetComponent<DustTrail>().EnableTrail();

            //basically a delay before the next dust can spawn
            EnableDustTrail();
            if (dustTrailIndex == 2)
            {
                dustTrailIndex = 0;
                return;
            }
        }
        dustTrailIndex++;
    }

    protected void EnableDustTrail()
    {
        stepRate = MaxStepRate;
        canSpawnDustTrail = false;
    }
    #endregion
}

