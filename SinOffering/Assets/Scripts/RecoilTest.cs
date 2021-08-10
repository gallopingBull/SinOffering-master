using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoilTest : MonoBehaviour
{
    public float RecoilAmmount_Grounded = 500;
    public float RecoilAmmount_Air = 500;

    private Entity entity;

    private void Awake()
    {
        entity = GetComponent<Entity>();
    }

    public void WeaponRecoil(int projectileDir)
    {
        entity.rb.velocity = Vector3.zero;

        if (entity.IsGrounded)
        {
            print("WeaponRecoil() called from RecoilTest.cs // NOT falling or jumping");
            if (entity.dir == 1)
            {
                if (projectileDir == 1) {
                    entity.rb.AddForce(entity.dir * RecoilAmmount_Grounded, 0, 0, ForceMode.VelocityChange);
                }
                else
                {
                    entity.rb.AddForce(-entity.dir * RecoilAmmount_Grounded, 0, 0, ForceMode.VelocityChange);
                }
            }
            else
            {
                if (projectileDir == 1)
                {
                    entity.rb.AddForce(-entity.dir * RecoilAmmount_Grounded, 0, 0, ForceMode.VelocityChange);
                }
                else {
                    entity.rb.AddForce(entity.dir * RecoilAmmount_Grounded, 0, 0, ForceMode.VelocityChange);
                }
                //entity.rb.AddForce((entity.dir * RecoilAmmount_Grounded), 0, 0, ForceMode.VelocityChange);
            }
            
        }
        else
        {
            print("WeaponRecoil() called from RecoilTest.cs // falling or jumping is true");
            if (entity.dir == 1)
            {
                if (projectileDir == 1)
                {
                    entity.rb.AddForce(entity.dir * RecoilAmmount_Air, 0, 0, ForceMode.VelocityChange);
                }
                else
                {
                    entity.rb.AddForce(-entity.dir * RecoilAmmount_Air, 0, 0, ForceMode.VelocityChange);
                }
            }
            else
            {
                if (projectileDir == 1)
                {
                    entity.rb.AddForce(-entity.dir * RecoilAmmount_Air, 0, 0, ForceMode.VelocityChange);
                }
                else
                {
                    entity.rb.AddForce(entity.dir * RecoilAmmount_Air, 0, 0, ForceMode.VelocityChange);
                }
                //entity.rb.AddForce((entity.dir * RecoilAmmount_Grounded), 0, 0, ForceMode.VelocityChange);
            }
            //entity.rb.AddForce(-entity.dir * RecoilAmmount_Air, 0, 0, ForceMode.VelocityChange);
        }
    }
}
