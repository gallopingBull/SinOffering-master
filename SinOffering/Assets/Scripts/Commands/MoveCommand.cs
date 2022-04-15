using UnityEngine;

/// <summary>
/// command for jump that's invoked by InputHandler.cs. implements jump behavior.
/// </summary>s

public class MoveCommand : Command 
{
    public override void Execute() => MoveActor();

    public override void Redo() { }

    public void MoveActor()
    {
        if (Time.timeScale == 1)
        {
            // deacelerate horizontal movement while player is in air falling allows for some Air Control
            if (_pc.state == Entity.State.falling || _pc.state == Entity.State.Jumping)
                _pc.rb.velocity = CalculateFallVelocity(_pc.dir, _pc.Speed, _pc.AirControl, Time.fixedDeltaTime, _pc.rb.velocity.y);

            // basic horizontal ground movement
            if (_pc.IsGrounded && _pc.state != Entity.State.dashing)
            {
                float aimingSpeedScale = 1;
                if (_pc.inputHandler.aiming)
                    aimingSpeedScale = .35f;
                _pc.rb.velocity = 
                    CalculateGroundVelocity(_pc.dir, _pc.Speed, aimingSpeedScale, Time.fixedDeltaTime, TimeScale.player, _pc.rb.velocity.y); 
                _pc.StateManager.EnterState(Entity.State.running);
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
}
