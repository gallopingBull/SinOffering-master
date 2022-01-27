using UnityEngine;
public class JumpCommand : ICommand {
    private void Awake()
    {
        pc = GetComponent<PlayerController>();
    }
    public override void Execute() { Jump(); }
    public override void Redo()
    {

    }
    
    public void Jump()
    {
        // return if reached max jump count
        if (pc.jumpCount > 1)
            return;

        if (pc.state == Entity.State.Jumping)
        {   
            // double jump
            if (pc.jumpCount == 1)
                DoubleJump();
            return;
        }

        // jump from ground 
        if (pc.IsGrounded  && pc.jumpCount == 0) //check if not in state jump state
        {
            // zero out y velocity
            pc.rb.velocity = new Vector3(pc.rb.velocity.x, 0, pc.rb.velocity.z);
        
            pc.rb.AddForce(Vector3.up * pc.JumpSpeed);
            // change state
            pc.StateManager.EnterState(Entity.State.Jumping);
            return;
        }

        // jump from falling state
        if (pc.state == Entity.State.falling)
        {
            //used to double jump after dash (needs fixin')  
            if (Input.GetButtonDown("Jump") && pc.jumpCount == 1)
            {
                DoubleJump();
                return;  
            }
            //print("calling jump from fallling state");
            if (pc.jumpCount == 0)
            {
                //print("jumping from falling state");
                // zero out y velocity
                pc.rb.velocity = new Vector3(pc.rb.velocity.x, 0,
                    pc.rb.velocity.z);

                pc.rb.AddForce(Vector3.up * pc.JumpSpeed);
                // change state
                pc.StateManager.EnterState(Entity.State.Jumping);
            }
        }
    }

    private void DoubleJump()
    {
        if (pc.CanDoubleJump)
        {
            SoundManager.PlaySound(pc.jumpClip);
            pc.jumpEnabled = false;
            pc.CanDoubleJump = false;
            
            //zero out y velocity
            pc.rb.velocity = new Vector3(pc.rb.velocity.x, 0, pc.rb.velocity.z);

            pc.rb.AddForce(Vector3.up * pc.JumpSpeed);
            pc.StateManager.EnterState(Entity.State.Jumping);
        }
    }
}
