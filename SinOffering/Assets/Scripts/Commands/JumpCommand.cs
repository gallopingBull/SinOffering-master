using UnityEngine;

/// <summary>
/// command for jump that's invoked by InputHandler.cs. implements jump behavior.
/// </summary>

public class JumpCommand : Command 
{
    private void Awake()
    {
        _pc = GetComponent<PlayerController>();
    }
    public override void Execute() => Jump();
    public override void Redo()
    {

    }
    
    public void Jump()
    {
        // return if reached max jump count
        if (_pc.jumpCount > 1)
            return;

        if (_pc.state == Entity.State.Jumping)
        {   
            // double jump
            if (_pc.jumpCount == 1)
                DoubleJump();
            return;
        }

        // jump from ground 
        if (_pc.IsGrounded  && _pc.jumpCount == 0) //check if not in state jump state
        {
            // zero out y velocity
            _pc.rb.velocity = new Vector3(_pc.rb.velocity.x, 0, _pc.rb.velocity.z);
        
            _pc.rb.AddForce(Vector3.up * _pc.JumpSpeed);
            // change state
            _pc.StateManager.EnterState(Entity.State.Jumping);
            return;
        }

        // jump from falling state
        if (_pc.state == Entity.State.falling)
        {
            //used to double jump after dash (needs fixin')  
            if (Input.GetButtonDown("Jump") && _pc.jumpCount == 1)
            {
                DoubleJump();
                return;  
            }
            //print("calling jump from fallling state");
            if (_pc.jumpCount == 0)
            {
                //print("jumping from falling state");
                // zero out y velocity
                _pc.rb.velocity = new Vector3(_pc.rb.velocity.x, 0, _pc.rb.velocity.z);
                _pc.rb.AddForce(Vector3.up * _pc.JumpSpeed);
                // change state
                _pc.StateManager.EnterState(Entity.State.Jumping);
            }
        }
    }

    private void DoubleJump()
    {
        if (_pc.CanDoubleJump)
        {
            SoundManager.PlaySound(_pc.jumpClip);
            _pc.jumpEnabled = false;
            _pc.CanDoubleJump = false;
            
            //zero out y velocity
            _pc.rb.velocity = new Vector3(_pc.rb.velocity.x, 0, _pc.rb.velocity.z);

            _pc.rb.AddForce(Vector3.up * _pc.JumpSpeed);
            _pc.StateManager.EnterState(Entity.State.Jumping);
        }
    }
}
