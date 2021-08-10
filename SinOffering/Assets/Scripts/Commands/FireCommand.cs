using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCommand : ICommand {

    public override void Execute() { FireWeapon(); }
    public override void Redo() { }
    
    private void FireWeapon()
    {    
        //this condition is for fully autmotic weapons, or weapons that require a trigger
        //to be held (laser, mines, etc.)
        if (Input.GetButton("Fire1") || Input.GetAxis("RightTrigger") > 0)
        {
            if (GetComponent<PlayerController>().weaponManager.CurWeapon == 2 ||
                GetComponent<PlayerController>().weaponManager.CurWeapon == 3 ||
                GetComponent<PlayerController>().weaponManager.CurWeapon == 5 ||
                GetComponent<PlayerController>().weaponManager.CurWeapon == 8)
            { 
                if (GetComponent<PlayerController>().EquippedWeapon.GetComponent<Weapon>().canFire)
                {
                    GetComponent<PlayerController>().EquippedWeapon.GetComponent<Weapon>().FireWeapon();
                }
            }
        }
        
        //fire button is pressed
        if ((Input.GetButtonDown("Fire1") | Input.GetAxis("RightTrigger") == 1) && 
            GetComponent<PlayerController>().EquippedWeapon.GetComponent<Weapon>().canFire)
        {
            GetComponent<PlayerController>().EquippedWeapon.GetComponent<Weapon>().FireWeapon();
        }
    }
}
