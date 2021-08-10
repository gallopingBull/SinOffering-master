using UnityEngine;
public class MoveCommand : ICommand {
    //private float xPos_before, yPos_before;
    private Vector3 prevPos;

    public override void Execute() {
        //xPos_before = GetComponent<GameObject>().transform.position.x;
        //prevPos = GetComponent<Transform>().position;
        MoveActor();
    }

    public override void Redo()
    {
        //MoveActorPrevPos();
    }

    public void MoveActor()
    {
     

        //basically check if dash isn't active
        if (Time.timeScale == 1)
        {

            //basic movement
            if (GetComponent<Entity>().IsGrounded &&
                GetComponent<PlayerController>().state != Entity.State.dashing)
            {
                /*
                if (GetComponent<PlayerController>().xRaw < -.01f &&
                    GetComponent<Entity>().Speed < GetComponent<Entity>().MaxSpeed)
                {
                    GetComponent<Entity>().Speed = GetComponent<Entity>().Speed + GetComponent<Entity>().AccSpeed;

                }
                else if (GetComponent<PlayerController>().xRaw > .01 && 
                    GetComponent<Entity>().Speed > -(GetComponent<Entity>().MaxSpeed))
                {
                    GetComponent<Entity>().Speed = GetComponent<Entity>().Speed + GetComponent<Entity>().AccSpeed;
                }
                else
                {
                    if (GetComponent<Entity>().Speed > (GetComponent<Entity>().DeAccSpeed * Time.fixedDeltaTime))
                    {
                         GetComponent<Entity>().Speed = GetComponent<Entity>().Speed - GetComponent<Entity>().DeAccSpeed;
                    }
                    else if (GetComponent<Entity>().Speed < (-GetComponent<Entity>().DeAccSpeed * Time.fixedDeltaTime))
                    {
                        GetComponent<Entity>().Speed = GetComponent<Entity>().Speed + GetComponent<Entity>().DeAccSpeed;
                    }
                    else
                    {
                        GetComponent<Entity>().Speed = 0;
                    }
                }
                */
                GetComponent<Entity>().rb.velocity = new Vector3(GetComponent<Entity>().dir * GetComponent<Entity>().Speed * (Time.fixedDeltaTime*TimeScale.player), GetComponent<Entity>().rb.velocity.y);

                GetComponent<PlayerController>().sm.EnterState(Entity.State.running);
            }

            //deacelerate horizontal movement while player is in air falling allows for some Air Control

            if (GetComponent<Entity>().state == Entity.State.falling ||
            GetComponent<Entity>().state == Entity.State.Jumping)
            {
                GetComponent<Entity>().rb.velocity = new Vector3(GetComponent<Entity>().dir * (GetComponent<Entity>().Speed * GetComponent<PlayerController>().AirControl) * Time.fixedDeltaTime,
                    GetComponent<Entity>().rb.velocity.y, 0);

                //change sprite/animation if player is holding weapon
                if (!GetComponent<PlayerController>().EquippedWeapon)
                {
                    if (!GetComponent<PlayerController>().animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Jump"))
                    {
                        GetComponent<PlayerController>().animator.Play("Player_Jump");
                    }
                }
                else
                {
                    //print("should be in player_jump_shoot");
                    GetComponent<PlayerController>().animator.Play("Player_Jump_Shoot");
                }
            }
        }
    }
    /**
    public void MoveActorPrevPos()
    {
        GetComponent<Entity>().rb.velocity = new Vector3(prevPos.x * GetComponent<Entity>().Speed, prevPos.y * GetComponent<Entity>().Speed, 0);
    }
    **/
}
