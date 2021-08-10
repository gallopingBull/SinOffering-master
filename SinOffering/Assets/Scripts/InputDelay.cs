using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDelay : MonoBehaviour
{
    #region variables
    [HideInInspector]
    public float evadeDelay = 0f;
    [HideInInspector]
    public float dashDelay = 0f;
    //[HideInInspector]
    public float jumpDelay = 0f;

    public float MAXEvadeDelay = .3f;
    public float MAXDashDelay = .3f;
    public float MAXjumpDelay = .3f;

    [HideInInspector]
    public bool evadeDelayComplete = true;
    [HideInInspector]
    public bool dashDelayComplete = true;
    //[HideInInspector]
    public bool jumpDelayComplete = true;

 
    private PlayerController pc;

    #endregion


    #region functions
    // Start is called before the first frame update
    private void Awake()
    {
        pc = GetComponent<PlayerController>();
    }

    public void InputDelayHandler(Entity.State _state)
    {
        switch (_state)
        {
            case Entity.State.evading:
                if (evadeDelay > 0)
                {
                    if (evadeDelayComplete)
                        evadeDelayComplete = false;
                    evadeDelay -= Time.deltaTime;
                }
                if(evadeDelay < 0)
                {
                    if (!evadeDelayComplete)
                        evadeDelayComplete = true;
                    evadeDelay = 0;
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

            case Entity.State.Jumping:
                if (jumpDelay > 0)
                {
                    jumpDelayComplete = false;
                    jumpDelay -= Time.deltaTime;
                    //if (jumpDelayComplete)   
                }
                if (jumpDelay < 0)
                {
                    jumpDelayComplete = true;
                    jumpDelay = 0;
                    //if (!jumpDelayComplete)
                }
                break;

            
            case Entity.State.falling:
            case Entity.State.Idle:
            case Entity.State.running:
                if (jumpDelay > 0f)
                {
                    print("resseting jump");
                    jumpDelay = 0;
                    jumpDelayComplete = true;
                }
                if (evadeDelay > 0f)
                {
                    print("resseting evade");
                    evadeDelay = 0;
                    evadeDelayComplete = true;
                }
                if (dashDelay > 0f)
                {
                    print("resseting dash");
                    dashDelay = 0;
                    dashDelayComplete = true;
                }
                
                break;

               
            default:
                break;

        }
      
    }

    #endregion
}
