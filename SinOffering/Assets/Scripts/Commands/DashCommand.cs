using System.Collections;
using System.Collections.Generic;
using UnitySpriteCutter;
using UnityEngine.UI;
using UnityEngine;

public class DashCommand : ICommand
{
    #region variables
    [HideInInspector]
    public enum DashState { init, inDashAttack, completed, disabled };
    //[HideInInspector]
    public DashState dashState;

    public RaycastHit2D[] m_Hits;
    public List<GameObject> targets;

    public float DashSpeed = 100f;  
    public float ShakeDur = .3f;
    public float ShakeAmmount = 07f;
    public GameObject dashLoc;
    public int MaxDashCount = 3;

    public float dashButtonHeldTime;
    [SerializeField]
    private float DashButtonHeldTimeMAX = .033f; //.02

    //[HideInInspector]
    public int dashCount;
    public Image[] dashImages;

    [HideInInspector]
    public bool EnableDashCommand;
    [HideInInspector]
    public bool isDashing = false;
    [HideInInspector]
    public bool dashAttack = false;

    private bool InCooldown;
    public bool isValid;
    public bool dashObstructed = false;

    public float CooldownTime;

    //private PlayerController pc; //reference to main Player Controller
    private Transform tmpPos; //store last position before initializing dash

    [HideInInspector]
    public GameObject RadialMenu;
    [HideInInspector]
    public Image RadialCounterBar;

    private float radialCounterValue = 0;
    private float radialCounterMulti = .0005f; //.05f;
    //[HideInInspector]
    public LineRenderer lr_DashTrajectory;
    public LineRenderer lr_DashAttack;
    public GameObject dashDestinationImage;

    private bool m_HitDetect;

    [SerializeField]
    private Collider[] m_Colliders;

    [SerializeField]
    private Collider2D[] m_Colliders2D;

    private Vector3 direction;
    private Vector3 origin;

    [SerializeField]
    private float circleCastRadius = .5f;
    [SerializeField]
    private float circleCastDashRadius = .5f;
    [SerializeField]
    private Vector3 BoxCastSize;
    private float curHitDistance;
    [Tooltip("original set to 4, testing with 7")]
    public float MaxHitDistance = 10; //original set to 4, testing with 7
    [Tooltip("this is for how close the player's dash dintination is to a wall/floor")]
    public float MaxDashDistanceDiff = 1.5f; 
    [SerializeField]
    public GameObject enemyTargetMarkers;

    public LayerMask layerMask;
    private PostProcessManager ppm;

    #endregion

    #region functions
    public override void Execute() {

        if (dashCount < MaxDashCount)
            EnableDashCommand = true;
        else
            EnableDashCommand = false;
    }
    public override void Redo() { }


    private void Awake()
    {
        targets = new List<GameObject>();
        pc = GetComponent<PlayerController>();
        //******\\
        //this is for dash. move back dashcomman afterwards
        RadialMenu = GameObject.Find("Radial Dash");
        RadialCounterBar = GameObject.Find("Radialbar").GetComponent<Image>();
        //******\\
        dashDestinationImage.SetActive(false);
        dashImages[0] = GameObject.Find("Image0").GetComponent<Image>();
        dashImages[1] = GameObject.Find("Image1").GetComponent<Image>();
        dashImages[2] = GameObject.Find("Image2").GetComponent<Image>();
    }

    private void Start()
    {
        ppm = PostProcessManager.intance;
        dashState = DashState.completed;
        RadialMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetAxis("LeftTrigger") == 0 &&
              !GameManager.instance.gameCompleted)
        {
            if (dashState == DashState.completed && 
                ppm.ppEnabled == true)
            {
                ppm.ExitInitDash(.75f, true);
                //print("dash completed and left trigger released");
            }

            if (dashState != DashState.completed &&
                dashButtonHeldTime > DashButtonHeldTimeMAX)
            {
                //cheeck if this condition is needed
                if (dashButtonHeldTime < DashButtonHeldTimeMAX)
                {
                    print("calling exitInitDash");
                    ppm.ExitInitDash(.75f, true);
                    DisableDashAttack();
                    return;
                }

                if (Time.timeScale != 1)
                {
                    Time.timeScale = 1;
                    TimeScale.DisableSlomo();
                }

                dashButtonHeldTime = 0;

                //reset radialCounterValue after releasing dash button
                if (radialCounterValue != 0 &&
                RadialMenu.activeSelf)
                {
                    radialCounterValue = 0;
                    RadialCounterBar.fillAmount = radialCounterValue;
                }

                pc.sm.ExitState(Entity.State.dashing);
                dashState = DashState.completed;

                if (dashState != DashState.inDashAttack &&
                    !EnableDashCommand)
                {
                    ppm.ExitInitDash(.75f, true);
                    DisableDashAttack();
                }
            }
        }


        if (EnableDashCommand)
        {
            //print("DashCommand.cs -> Update() at: " + Time.realtimeSinceStartup);
            if (Input.GetAxis("LeftTrigger") == 1)
            {
                //print("holding left trigger");
                if (dashState == DashState.inDashAttack)
                {
                    return;
                }
                if (dashCount == MaxDashCount)
                {
                    print("max cound reached - LEftTrigger being held");
                    DisableDashAttack();
                    return;
                }

                if (dashState != DashState.inDashAttack
                && pc.state != Entity.State.dashing)
                {
                    dashButtonHeldTime += .01f;
                    if (dashButtonHeldTime > DashButtonHeldTimeMAX)
                    {
                        if (dashCount <= MaxDashCount)
                        {
                            InitDash();
                        }

                        if (radialCounterValue < 1 && RadialMenu.activeSelf)
                        {
                            CalculateDashTrajectory();
                            radialCounterValue += radialCounterMulti;
                            RadialCounterBar.fillAmount = radialCounterValue;
                        }

                        //if dash attack button is held too long
                        //activate dash attack
                        if (radialCounterValue >= 1)
                        {
                            if (dashState != DashState.inDashAttack)
                            {
                                radialCounterValue = 0f;
                                RadialCounterBar.fillAmount = radialCounterValue;

                                if (dashCount <= MaxDashCount &&
                                    RadialMenu.activeInHierarchy)
                                {
                                    if (Time.timeScale != 1)
                                    {
                                        Time.timeScale = 1;
                                        TimeScale.DisableSlomo();
                                    }


                                    dashDestinationImage.SetActive(false);

                                    pc.rb.velocity = Vector3.zero;
                                    //print("call dash fronm Update()");
                                    if (targets != null)
                                    {
                                        dashAttack = true;
                                    }

                                    Dash();
                                    RadialMenu.SetActive(false);
                                }
                            }
                        }
                    }
                }
            }

            if (Input.GetAxis("RightTrigger") == 1 && isValid
                && !GameManager.instance.gameCompleted && 
                (direction.x != 0 || direction.y != 0) && 
                pc.inputHandler.dashDelayComplete)
            {
                //print(dashState);
                if (dashState == DashState.init)
                {
                    //print("In DashCommand.cs -> Update() -> Right Triggr Pressed");
                    //print("origin: " + origin + "; origin + (direction.normalized * curHitDistance): " + (origin + (direction.normalized * curHitDistance)));
                    if (Time.timeScale != 1)
                    {
                        Time.timeScale = 1;
                        TimeScale.DisableSlomo();
                    }

                    lr_DashTrajectory.gameObject.SetActive(false);
                    lr_DashAttack.gameObject.SetActive(false);
                    dashDestinationImage.SetActive(false);

                    if (targets.Count > 0)
                        dashAttack = true;
                    
                    Dash();
                    dashButtonHeldTime = 0;

                    //reset radialCounterValue after releasing dash button
                    if (radialCounterValue != 0 &&
                    RadialMenu.activeSelf)
                    {
                        radialCounterValue = 0;
                        RadialCounterBar.fillAmount = radialCounterValue;
                    }
                }
            }
        }
        else
        {
            //ppm.ExitInitDash(.75f, true);
            if (dashCount == MaxDashCount &&
                Time.timeScale != 1)
            {
                DisableDashAttack();
                ppm.ExitInitDash(.75f, true);
                print("max cound reached");
            }
            /*
            if (dashState != DashState.disabled)
            {
                dashState = DashState.disabled;
            }*/
        }
    }

    //called when dash trigger is held down
    public void InitDash()
    {
        if (!pc.dying && dashState != DashState.init)
        {
            //slow down time
            if (Time.timeScale != .1)
            {
                Time.timeScale = .1f;
                TimeScale.EnableSlomo();
            }
            ppm.InitDash(); //darken screen or some other sfx to emphasize player
            dashState = DashState.init;

            GetComponent<Entity>().rb.useGravity = false;
            GetComponent<Entity>().rb.velocity = Vector3.zero;

            if (pc.transform != tmpPos)
                tmpPos = pc.transform;
            else
                pc.transform.position = tmpPos.position;

            //bring up dash wheel / line trajectory
            if (RadialMenu != null)
                RadialMenu.SetActive(true);
            lr_DashTrajectory.gameObject.SetActive(true);
            lr_DashAttack.gameObject.SetActive(true);
        }
    }

    private void Dash()
    {
        RadialMenu.SetActive(false);

        if (lr_DashTrajectory.gameObject.activeSelf)
        {
            lr_DashTrajectory.gameObject.SetActive(false);
            lr_DashAttack.gameObject.SetActive(false);
        }

        pc.inputHandler.dashDelay = pc.inputHandler.MAXDashDelay;


        if (dashState != DashState.inDashAttack
            && dashCount <= MaxDashCount)
        {
            dashCount++;
            DashUI(false);
            StartCoroutine(AutoCoolDown());
            StartCoroutine(Dashing());
        }
    }

    private void DashToLocation()
    {
        if (GetDashDirection().x != 0 || GetDashDirection().y != 0)
        {
            pc.transform.position = origin + (direction.normalized * curHitDistance);
        }
        else
        {
            Vector3 tmpVel;
            if (pc.facingLeft)
            {
                tmpVel = new Vector3(-1 * DashSpeed, transform.position.y, transform.position.z);
            }
            else
            {
                tmpVel = new Vector3(1, transform.position.y, transform.position.z);
            }

            transform.position = origin + (direction.normalized * curHitDistance);
        }
    }
    private IEnumerator Dashing()
    {
        dashState = DashState.inDashAttack;
        pc.sm.EnterState(Entity.State.dashing);

        if (targets.Count > 0)
        {
            FreezeEnemies(targets); //freeze enemies that will be dash killed

            yield return new WaitForSeconds(.5f);
        }
        ppm.OnDash(.75f, 5f, true);
        DashToLocation();
        
        CameraShake.instance.Shake(ShakeDur, ShakeAmmount, true);

        yield return new WaitForSeconds(.25f);

        if (Time.timeScale != .1)
        {
            Time.timeScale = .1f;
            TimeScale.EnableSlomo();
        }

        if (targets.Count > 0 && dashAttack)
        {
            var enemyTotal = targets.Count;
    
            Vector3 tmpDir = (direction.normalized * curHitDistance);
            Vector3 tmpEnd = origin + (direction.normalized * curHitDistance);

            GetComponent<ISlice>().Slice(origin, tmpEnd, layerMask, targets, tmpDir);
            
            //yield return new WaitForSeconds(.01f);
            DestroyEnemies();
            yield return new WaitForSeconds(enemyTotal * .025f); // adds a small delay before ending slomo cinematic dash attack
                                                                    // delay is scaled by how many enemies are dash killed (.015-.0025 )
        }

        if (!InCooldown && dashCount == MaxDashCount)
        {
            InCooldown = true;
            StartCoroutine(CoolDown());
        }

        yield return new WaitForSeconds(.01f); //delay before dash attack finishes
        DisableDashAttack();

        pc.sm.ExitState(Entity.State.dashing);
        dashState = DashState.completed;

        StopCoroutine(Dashing());
    }
    public void DisableCollisions()
    {
        foreach (Collider collider in m_Colliders)
        {
            collider.enabled = false;
        }

        foreach (Collider2D collider in m_Colliders2D)
        {
            collider.enabled = false;
        }
    }
    public void EnableCollisions()
    {
        foreach (Collider collider in m_Colliders)
        {
            collider.enabled = true;
        }
        foreach (Collider2D collider in m_Colliders2D)
        {
            collider.enabled = true;
        }
    }
    private Vector3 GetDashDirection()
    {
        float xRaw = pc.xRaw;
        float yRaw = pc.yRaw;
        return new Vector3(xRaw, yRaw, 0);
    }
    
    public void CalculateDashAttackTargets(Vector2 _origin, Vector2 _dir, float _hitDistance)
    {
        List<GameObject> gameObjectsToCut = new List<GameObject>();
        //Remove any 'X' on enemies that have been destroyed
        if (targets != null)
        {
            RemoveDashTags();
        }
        
        //RaycastHit2D[] hits = Physics2D.CircleCastAll(_origin, circleCastRadius, _dir,_hitDistance, layerMask);
        RaycastHit2D[] hits = Physics2D.RaycastAll(_origin,  _dir,_hitDistance, layerMask);
        foreach (RaycastHit2D hit in hits)
        {
            GameObject tmp = hit.transform.gameObject;
            if (tmp.tag == "Enemy")
            {
                if (targets == null &&
                    !tmp.GetComponent<EnemyController>().dying)
                {
                    targets.Add(tmp);
                    break;
                }
                if (tmp != null &&
                    !tmp.transform.parent.GetComponent<EnemyController>().dying)
                {
                    if (targets.Contains(tmp))
                        continue;
                    else
                    {
                        Instantiate(enemyTargetMarkers,
                            tmp.transform.parent.transform.position,
                            tmp.transform.parent.transform.rotation,
                            tmp.transform.parent.transform);
                        targets.Add(tmp.transform.parent.gameObject);
                    }
                }
                //maybe delete this
                if (hit.distance > MaxDashDistanceDiff)
                {
                    dashObstructed = false;
                }
            }    
        }
    }
    public void CalculateDashTrajectory()
    {
        isValid = false;
        origin = new Vector3(pc.transform.position.x,
            pc.transform.position.y + .1f,
            pc.transform.position.z);
        direction = GetDashDirection();
        curHitDistance = MaxHitDistance;


        m_Hits = Physics2D.BoxCastAll(origin, BoxCastSize, 0,direction, MaxHitDistance, layerMask);
        

        //print(m_Hits.Length);
        foreach (RaycastHit2D hit in m_Hits)
        {
           //print(" name: " + hit.transform.gameObject.name + 
             //   " || tag: " + hit.transform.gameObject.tag);
            
            GameObject tmp = hit.transform.gameObject;
          
            if ((hit.transform.gameObject.tag == "Floor" ||
            hit.transform.gameObject.tag == "Wall"))
            {
                if (hit.distance ==0)
                {
                    dashObstructed = false;

                }
                //print("TOO CLOSE TO WALL - DASH FAILED");
                if (hit.distance < MaxDashDistanceDiff)
                    dashObstructed = true;

                //print("far enough from  wall/ground to dash");
                else
                    dashObstructed = false;

                curHitDistance = hit.distance;
                if (curHitDistance == 0)
                    print("sjit");

                else
                {
                    break;
                }                   
            }
        }
   
        if (m_Hits.Length > 0)
        {
            if (!isValid && !dashObstructed)
                isValid = true;
        }

        if (m_Hits.Length == 0)
        {
            dashObstructed = false;
            if (!isValid && !dashObstructed)
                isValid = true;
        }

        lr_DashTrajectory.SetPosition(0, origin);
        lr_DashAttack.SetPosition(0, origin);

        lr_DashTrajectory.SetPosition(1, origin + ((direction.normalized * curHitDistance)*.95f)); //.9-.95 tp scale the size of the "outer rim" of the dash trajectory 
                                                                                                  //so its slightly shorter than the lr_dashAttack

        
        lr_DashAttack.SetPosition(1, origin + (direction.normalized * curHitDistance)*1.11f);
        DashTrajectoryMarker(origin + (direction.normalized * curHitDistance) * 1.11f);

        CalculateDashAttackTargets(origin, direction, curHitDistance);
    }
    private void DashTrajectoryMarker(Vector3 hitLoc)
    {
        dashDestinationImage.transform.position = hitLoc;

        if (!dashDestinationImage.activeSelf && 
            dashCount <= MaxDashCount && dashState == DashState.init)
            dashDestinationImage.SetActive(true);

        if (direction.x < -0.01f)
            dashDestinationImage.GetComponent<SpriteRenderer>().flipX = true;
        else
            dashDestinationImage.GetComponent<SpriteRenderer>().flipX = false;
    }

    private void OnDrawGizmos()
    {
        //for dash trajectory
        if (dashObstructed)
        {
            Gizmos.color = Color.red;

            Debug.DrawRay(origin, direction.normalized * curHitDistance, Color.red);
            Gizmos.DrawWireCube(origin + (direction.normalized * curHitDistance), BoxCastSize);
            //Gizmos.DrawWireSphere(origin + (direction.normalized * curHitDistance), circleCastDashRadius);
        }
        else
        {

            Gizmos.color = Color.green;

            Debug.DrawRay(origin, direction.normalized * curHitDistance, Color.green);
            Gizmos.DrawWireCube(origin + (direction.normalized * curHitDistance), BoxCastSize);
            //Gizmos.DrawWireSphere(origin + (direction.normalized * curHitDistance), circleCastDashRadius);
        }

        //for dash attack
        Gizmos.color = Color.yellow;
        Debug.DrawRay(origin, direction.normalized * curHitDistance, Color.yellow);
        Gizmos.DrawWireSphere(origin + (direction.normalized * curHitDistance), circleCastRadius);
    }

    public void ChangeRigidbodyValues()
    {
        if (pc.yRaw < 0)
        {
            if (pc.state == Entity.State.falling ||
                pc.state == Entity.State.Jumping)
            {
                //print("dashing down while falling");
                //print("1st cond. || player drag: 1000 || player state: " + GetComponent<PlayerController>().state);
                pc.rb.mass = .01f;
                pc.rb.angularDrag = 100f;
                pc.rb.drag = 1000f;
            }
        }
        if (pc.yRaw < .001f && pc.state != Entity.State.running)
        {
            //print("2nd cond. || player drag: 15 || player state: " + GetComponent<PlayerController>().state);
            pc.rb.drag = 30;
        }
        else
        {
            //print("3rd cond. || player drag: 30 || player state: " + GetComponent<PlayerController>().state);
            pc.rb.drag = 30;
        }
    }

    //displays and changes dash counter in UI
    private void DashUI(bool coolingDown)
    {
        if (!coolingDown)
        {
            for (int i = 2; i >= 0; i--)
            {
                if (dashImages[i].fillAmount == 1)
                {
                    dashImages[i].fillAmount = 0;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < dashImages.Length; i++)
            {
                if (dashImages[i].fillAmount == 0)
                {
                    dashImages[i].fillAmount = 200;
                }
            }
        }
    }

    private IEnumerator AutoCoolDown()
    {   
        yield return new WaitForSeconds(CooldownTime * 2f);
        if (!InCooldown && dashCount > MaxDashCount &&
            pc.state != Entity.State.dashing)
        {
            //print("auto cooldown finished");
            InCooldown = true;
            StartCoroutine(CoolDown());
        }

        //print("auto cooldown finished");
        StopCoroutine(AutoCoolDown());
    }

    private IEnumerator CoolDown()
    {
        StopCoroutine(AutoCoolDown());
        yield return new WaitForSeconds(CooldownTime);

        dashCount = 0;
        DashUI(true);
        InCooldown = false;

        StopCoroutine(CoolDown());
    }

    private void FreezeEnemies(List<GameObject> dashTargets)
    {
        //stop movement for all dash attack targets
        foreach (GameObject target in dashTargets)
        {
            if (target != null)
            {
                target.GetComponent<EnemyController>().rb.velocity = Vector3.zero;
                target.GetComponent<EnemyController>().rb.isKinematic = true;
                target.GetComponent<EnemyController>().rb.useGravity = false;
                target.GetComponentInChildren<Rigidbody2D>().velocity = Vector2.zero;
                target.GetComponentInChildren<Rigidbody2D>().isKinematic = true;
                target.GetComponentInChildren<Rigidbody2D>().gravityScale = 0;
                target.GetComponent<EnemyController>().CanMove = false;
            }
        }
    }
    
    //remove tags on enemies that were marked for a dash attack
    private void RemoveDashTags()
    {
        foreach (GameObject target in targets)
        {
            if (target != null)
            {
                Destroy(target.transform.Find("X(Clone)").gameObject);
            }
        }
        targets.Clear();
    }
    private void DestroyEnemies()
    {
        List<GameObject> tmpTargetList = targets;

        if (tmpTargetList.Count > 0)
        {
            #region testing / want slice here
            // adds a small delay before ending slomo cinematic dash attack
            // delay is scaled by how many enemies are dash killed (.015-.0025 )
            //GetComponent<ISlice>().Slice(origin,
            //origin + (direction.normalized * curHitDistance), layerMask);
            //yield return new WaitForSeconds(tmpTargetList.Count * .025f);
            #endregion

            for (int i = 0; i < tmpTargetList.Count; i++)
            {
                if (tmpTargetList[i] != null)
                { 
                    //RemoveEnemyFromLists(target); //testing this location
                    //LoopUpdate(tmpTargetList, i, false); //prints elements in list for debuggin purposes
                    //print("i: " + i);
                    if (tmpTargetList.Count == 0)
                    {
                        break;
                    }
                    else
                    {
                        //print("Calling DashKilled() on this enemy target: " + targets[i].name);

                        if (tmpTargetList[i] == null)
                        {
                            break;
                        }
                        tmpTargetList[i].GetComponent<EnemyController>().DashKilled();
                    }
                }
            }
        }
        dashAttack = false;
        targets.Clear();
    }
    
    public void DisableDashAttack()
    {
        if (!EnableDashCommand)
        {
            RadialMenu.SetActive(false);
            dashDestinationImage.SetActive(false);
            lr_DashTrajectory.gameObject.SetActive(false);
            lr_DashAttack.gameObject.SetActive(false);

            RemoveDashTags();
            //print("let go of trigger");

            isValid = false;

            dashButtonHeldTime = 0;
            Time.timeScale = 1;
            TimeScale.DisableSlomo();
        }
    }

    #endregion

}
