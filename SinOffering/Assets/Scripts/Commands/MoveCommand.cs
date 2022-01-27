using UnityEngine;
public class MoveCommand : ICommand {
    private Vector3 _prevPos;
    public override void Execute() {
        //prevPos = GetComponent<Transform>().position;
        MoveActor();
    }

    public override void Redo()
    {
        //MoveActorPrevPos();
    }

    public void MoveActor()
    {
        if (Time.timeScale == 1)
        {
            // deacelerate horizontal movement while player is in air falling allows for some Air Control
            if (pc.state == Entity.State.falling || pc.state == Entity.State.Jumping)
            {
                pc.rb.velocity = new Vector3(pc.dir * (pc.Speed * pc.AirControl) * Time.fixedDeltaTime, pc.rb.velocity.y, 0);
            }

            // basic horizontal ground movement
            if (pc.IsGrounded && pc.state != Entity.State.dashing)
            {
                #region check if i need this
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
                #endregion
                float aimingSpeedScale = 1;
                if (pc.inputHandler.aiming)
                    aimingSpeedScale = .35f;
                pc.rb.velocity = 
                    new Vector3(pc.dir * (pc.Speed*aimingSpeedScale) * (Time.fixedDeltaTime*TimeScale.player), pc.rb.velocity.y);
                pc.StateManager.EnterState(Entity.State.running);
            }
        }
    }

    #region Redo() Testing
    /**
    public void MoveActorPrevPos()
    {
        pc.rb.velocity = new Vector3(prevPos.x * pc.Speed, prevPos.y * pc.Speed, 0);
    }
    **/
    #endregion

}
