﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    private PlayerController pc;
    private void Awake()
    {
        pc = GetComponent<PlayerController>();
    }
    public void EnterState(Entity.State _state)
    {
        ExitState(pc.state);
        //Debug.Log("exiting state: " + pc.state);
        //Debug.Log("entering state: " + _state);
        switch (_state)
        {
            case Entity.State.Idle:
                if (!pc.EquippedWeapon)
                    pc.animator.Play("Player_Idle");
                else
                    pc.animator.Play("Player_Shoot");
                pc.state = Entity.State.Idle; // set state to idle 
                break;

            case Entity.State.Jumping:
                pc.state = Entity.State.Jumping; // set state to jump 
                if (pc.IsGrounded)
                    pc.IsGrounded = false;

                // change sprite/ animation if weapon equipped
                if (!pc.EquippedWeapon)
                {
                    if (!pc.animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Jump"))
                        pc.animator.Play("Player_Jump");
                }
                else
                {
                    if (!pc.animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Jump_Shoot"))
                        pc.animator.Play("Player_Jump_Shoot");
                }
                GetComponent<InputHandler>().jumpDelay = GetComponent<InputHandler>().MAXjumpDelay;
 
                #region testing
                //pc.InputDelay.jumpDelay = 
                //  pc.InputDelay.MAXjumpDelay;
                #endregion
                    
                SoundManager.PlaySound(pc.jumpClip);
                if (pc.jumpCount == 0)
                    pc.CanDoubleJump = true;
                
                pc.jumpCount++;
                Debug.Log("pc.jumpCount: " + pc.jumpCount);
                break;

            case Entity.State.dashing:
                //animator.Play("Player_Dashing");
                pc.rb.velocity = Vector3.zero;
                pc.state = Entity.State.dashing;
                pc.isInvincible = true;
                pc.DisableInput();
                pc.jumpEnabled = false;
                pc.CanDoubleJump = false;
                //GetComponent<PlayerController>().ps.Play();
                GetComponent<Entity>().rb.useGravity = false;

                GetComponent<DashCommand>().EnableDashCommand = false; // i feel like keeping this in dashcommand
                GetComponent<DashCommand>().DisableCollisions();
                GetComponent<DashCommand>().ChangeRigidbodyValues(); // adjust entity's rigigidbody values in order to dash correctly

                SoundManager.PlaySound(pc.dashClip);

                break;

            case Entity.State.aiming:
                //animator.Play("Player_Shoot");
                //print("Player     is shooting");
                break;

            case Entity.State.running:
                if (pc.state != Entity.State.running)
                    pc.state = Entity.State.running;

                // change sprite/animation if player is holding weapon
                if (pc.EquippedWeapon == null)
                    pc.animator.Play("Player_Run");
                else
                {
                    if (pc.inputHandler.aiming)
                        break;
                    pc.animator.Play("Player_Run_Shoot");

                }
                    
                break;

            case Entity.State.falling:
                if (pc.EquippedWeapon == null)  
                    pc.animator.Play("Player_Falling");
                else
                    pc.animator.Play("Player_Jump_Shoot");
                pc.state = Entity.State.falling;
                break;

            case Entity.State.evading:
                print("Player is evading");
                pc.animator.Play("Player_Evade_Back");
                pc.DisableInput();
                pc.rb.velocity = Vector3.zero;

                //pc.InputDelay.evadeDelay =
                //pc.InputDelay.MAXEvadeDelay;
                pc.state = Entity.State.evading;
                pc.isInvincible = true;
                pc.jumpEnabled = false;
                pc.CanDoubleJump = false;
                //GetComponent<PlayerController>().ps.Play();
                pc.rb.useGravity = false;

                GetComponent<DashCommand>().DisableCollisions();
                GetComponent<DashCommand>().ChangeRigidbodyValues(); //adjust entity's rigigidbody values in order to dash correctly

                SoundManager.PlaySound(pc.dashClip);
                break;
        }
        //Debug.Log("new state: " + pc.state);
    }
    public void ExitState(Entity.State _state)
    {
        switch (_state)
        {
            case Entity.State.Idle:
                //print("Player state is exiting 'idle' state");
                break;

            case Entity.State.Jumping:

                //change sprite/ animation if weapon 
                //print("Player state is exiting 'jump' state");

                break;

            case Entity.State.dashing:
                //animator.Play("Player_Dashing");
                //GetComponent<PlayerController>().ps.Stop();
                pc.rb.useGravity = true;
                pc.rb.velocity = Vector3.zero;
                pc.rb.mass = 1f;
                pc.rb.drag = 0;
                pc.rb.angularDrag = 0f;
                pc.jumpEnabled = true;
                pc.isInvincible = false; 
                pc.EnableInput();

                GetComponent<DashCommand>().EnableCollisions();
                GetComponent<DashCommand>().EnableDashCommand = false; //i feel like keeping this in dashcommand

                break;

            case Entity.State.aiming:
                //animator.Play("Player_Shoot");
                //print("Player state is exiting 'fire' state");
                break;

            case Entity.State.running:
                //print("Player is running");
                //print("Player state is exiting 'running' state");
                break;

            case Entity.State.falling:
                //print("Player state is exiting 'falling' state");
                break;
            case Entity.State.evading:
                //print("Player is exitinf evading");
                pc.rb.velocity = Vector3.zero;
                pc.rb.useGravity = true;

                pc.rb.mass = 1f;
                pc.rb.drag = 0;
                pc.rb.angularDrag = 0f;
                pc.jumpEnabled = true;
                pc.isInvincible = false;
                pc.EnableInput();
                GetComponent<DashCommand>().EnableCollisions();
                if (GetComponent<EvadeCommand>().EvadeCount == 2)
                {
                    GetComponent<EvadeCommand>().EvadeCount = 0;
                    GetComponent<EvadeCommand>().AirEvadeCount = 0; 
                }

                break;
        }
    }
    // Updates state every frame - place in PlayerController's FixedUpdate();
    private void UpdateState()
    {
    }
}
