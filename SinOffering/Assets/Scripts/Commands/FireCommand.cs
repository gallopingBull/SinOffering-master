using UnityEngine;

/// <summary>
/// 
/// </summary>

public class FireCommand : Command 
{
    public override void Execute() => FireWeapon();

    public override void Redo() { }

    private void FireWeapon()
    {
        // this condition is for fully autmotic weapons, or weapons that require a trigger
        // to be held (laser, mines, etc.)
        if (Input.GetAxis("RightTrigger") > 0)
        {
            if (_pc.weaponManager.CurWeapon == 2 || _pc.weaponManager.CurWeapon == 3 ||
                _pc.weaponManager.CurWeapon == 5 || _pc.weaponManager.CurWeapon == 8)
            {
                if (_pc.EquippedWeapon.GetComponent<Weapon>().canFire)
                    _pc.EquippedWeapon.GetComponent<Weapon>().FireWeapon();
            }
        }

        // fire button is pressed
        if (Input.GetAxis("RightTrigger") == 1 && _pc.EquippedWeapon.GetComponent<Weapon>().canFire)
            _pc.EquippedWeapon.GetComponent<Weapon>().FireWeapon();

    }
}
