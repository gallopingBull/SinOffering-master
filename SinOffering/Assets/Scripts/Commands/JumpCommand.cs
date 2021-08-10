using UnityEngine;
public class JumpCommand : ICommand {
    
    public override void Execute() { Jump(); }
    public override void Redo()
    {

    }
    private void Awake()
    {
        pc = GetComponent<PlayerController>();
    }
    public void Jump()
    {
        //return if reached max jump count
        if (pc.jumpCount > 1)
        {
            return;
        }


        //jump from ground 
        if (pc.IsGrounded  && 
            pc.jumpCount == 0) //check if not in state jump state
        {
            //zero out y velocity
            pc.rb.velocity = new Vector3(pc.rb.velocity.x, 0,
                pc.rb.velocity.z);
        
            pc.rb.AddForce(Vector3.up * pc.JumpSpeed);
            //change state
            pc.sm.EnterState(Entity.State.Jumping);
            return;
        }

        //jump from falling state
        if (pc.state == Entity.State.falling)
        {
            //used to double jump after dash (needs fixin')  
            if (Input.GetButtonDown("Jump") &&
                pc.jumpCount == 1)
            {
                //print("jumpcount == 1");
                //zero out y velocity
                pc.rb.velocity = new Vector3(pc.rb.velocity.x, 0,
                    pc.rb.velocity.z);
                    
                pc.rb.AddForce(Vector3.up * pc.JumpSpeed);
                //change state
                pc.sm.EnterState(Entity.State.Jumping);
                return;
               
            }

            //print("calling jump from fallling state");
            if (pc.jumpCount == 0)
            {
                //print("jumpcount ==0");
                //delete this after testing
                if (pc.state == Entity.State.Jumping)
                {
                    //print("precheck: player is grounded and jump count == 0" + " || player state: " + GetComponent<PlayerController>().state);
                }

                if (pc.state != Entity.State.Jumping)
                {
                    //print("jumping from falling state");
                    //zero out y velocity
                    pc.rb.velocity = new Vector3(pc.rb.velocity.x, 0,
                        pc.rb.velocity.z);

                    pc.rb.AddForce(Vector3.up * pc.JumpSpeed);
                    //change state
                    pc.sm.EnterState(Entity.State.Jumping);
                }
                return;
            }

        }
        
        //double jump
        if (Input.GetButtonDown("Jump") && pc.jumpCount > 0)
        {
            DoubleJump();
            return;
        }
    }

    private void DoubleJump()
    {
        if (pc.CanDoubleJump)
        {
            //print("in double jump");
            SoundManager.PlaySound(pc.jumpClip);
            pc.jumpEnabled = false;
            pc.CanDoubleJump = false;
            
            //zero out y velocity
            pc.rb.velocity = new Vector3(pc.rb.velocity.x, 0,
                pc.rb.velocity.z);

            pc.rb.AddForce(Vector3.up * pc.JumpSpeed);
            pc.jumpCount++;

        }
    }
}
