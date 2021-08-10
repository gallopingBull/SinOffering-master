using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    public float RecoilAmmount_Grounded = 3;
    public float RecoilAmmount_Air = 3;

    private PlayerController pc;

    private void Awake()
    {
        pc = PlayerController.instance;
    }

    public void WeaponRecoil()
    {
        pc.rb.velocity = Vector3.zero; 
        if (pc.state == Entity.State.falling ||
            pc.state == Entity.State.Jumping)
        {
            pc.rb.AddForce(-pc.dir * RecoilAmmount_Air, 0, 0, ForceMode.VelocityChange);
        }
        else
        {
            pc.rb.AddForce((-pc.dir * RecoilAmmount_Grounded) / 4, 0, 0, ForceMode.VelocityChange);
        }
    }
}
