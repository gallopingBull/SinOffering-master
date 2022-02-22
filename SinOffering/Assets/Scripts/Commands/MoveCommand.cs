using UnityEngine;
public class MoveCommand : ICommand {
    private Vector3 _prevPos;
    public override void Execute() 
    {
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
                pc.rb.velocity = CalculateFallVelocity(pc.dir, pc.Speed, pc.AirControl, Time.fixedDeltaTime, pc.rb.velocity.y);

            // basic horizontal ground movement
            if (pc.IsGrounded && pc.state != Entity.State.dashing)
            {
                float aimingSpeedScale = 1;
                if (pc.inputHandler.aiming)
                    aimingSpeedScale = .35f;
                pc.rb.velocity = 
                    CalculateGroundVelocity(pc.dir, pc.Speed, aimingSpeedScale, Time.fixedDeltaTime, TimeScale.player, pc.rb.velocity.y); 
                pc.StateManager.EnterState(Entity.State.running);
            }
        }
    }

    private Vector3 CalculateGroundVelocity(int dir, float speed, float aimSpeedScale, float fixedDeltaTime, float timeScale, float fallSpeed)
    {
        return new Vector3(dir * (speed * aimSpeedScale) * (fixedDeltaTime * timeScale), fallSpeed);
    }
    private Vector3 CalculateFallVelocity(int dir, float speed, float airControl, float fixedDeltaTime, float fallSpeed)
    {
        return new Vector3(dir * (speed * airControl) * fixedDeltaTime, fallSpeed, 0);
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
