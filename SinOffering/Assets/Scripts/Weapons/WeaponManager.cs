using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Maybe change the name of this class to WeaponInventory.cs
public class WeaponManager : MonoBehaviour {
    public int CurWeapon;
    public bool WeaponEquipped = false;
    public GameObject[] Weapons;
    public Transform LHandSocket, RHandSocket; 
    public Transform mainHandSocket; 

    private PlayerController pc; 

    // Use this for initialization
    void Awake () { pc = GetComponent<PlayerController>(); }

    private void Start()
    {
        Invoke("InitWeapons", .2f);    
    }

    private void LateUpdate()
    {
        //UpdateWeaponSocket();
    }

    // give every weapon a default "temp" weaponattribute 
    // if one isn't read in during the load process
    private void InitWeapons()
    {
        for (int i = 0; i < Weapons.Length; i++)
        {
            //print(Weapons[i].name + " being intilized");
            Weapons[i].GetComponent<Weapon>().SetTempAttributeData();
        }
    }
    public void EquipWeapon(int newWeapon)
    {   
        if (pc.EquippedWeapon != null)
        {
            pc.ResetSpeed(); //reset player to default speed
            pc.EquippedWeapon.SetActive(false);
        }
        
        CurWeapon = newWeapon;
        pc.EquippedWeapon = Weapons[CurWeapon];
        // set weapon position/rotation
        pc.EquippedWeapon.SetActive(true);
        pc.EquippedWeapon.GetComponent<Weapon>().InitWeapon();
        WeaponEquipped = true;
        // have the weapon call InitWeapon()
        // wihtin itself instead
    }
    

    private void UpdateWeaponSocket()
    {
        if (pc.inputHandler.aiming)
        {
            if (pc.inputHandler._aimDir != 1)
            {
                Vector3 tmpPos = pc.EquippedWeapon.transform.localPosition;
                //Vector3 tmpRot = pc.EquippedWeapon.transform.eulerAngles;
                tmpPos.x *= -100;
                //tmpRot.z *= -1;
                //Debug.Log("tmp.x: " + tmpPos.x);
                pc.EquippedWeapon.transform.localPosition = tmpPos;
            }
            return;
        }
        if (pc.dir != 1)
        {
            //Debug.Log("in update");   
            Vector3 tmpPos = pc.EquippedWeapon.transform.localPosition;
            //Vector3 tmpRot = pc.EquippedWeapon.transform.eulerAngles;
            tmpPos.x *= -100;
            //tmpRot.z *= -1;
            //Debug.Log("tmp.x: " + tmpPos.x);
            pc.EquippedWeapon.transform.localPosition = tmpPos;
            //pc.EquippedWeapon.transform.eulerAngles = tmpRot;
        }
    }

    //https://github.com/mucahits/rotateintervally/blob/master/RotateIntervally.cs
    // rotate intervally 
    public float GetTargetEuler(Vector3 touchPosition, float interval)
    {
        float currentAngle = Mathf.Atan2(touchPosition.y, touchPosition.x) * Mathf.Rad2Deg;
        currentAngle = (currentAngle > 0) ? currentAngle : currentAngle + 360f;

        var region = (int)Mathf.Floor(currentAngle / interval);

        return region * interval;
    }

    public void ModifyWeaponRotation(int dir, Vector3 angle)
    {
        Weapons[CurWeapon].transform.rotation = 
            Quaternion.Euler(0,0, GetTargetEuler(angle * dir, 45f) );
    }

    public void ChangeWeapon()
    {
        if (Input.GetButtonDown("SwapWeapon"))
        {
            if (CurWeapon != Weapons.Length - 1)
                CurWeapon++;
            else
                CurWeapon = 0;
        }
        #region old - might delte later
        /*if (Input.GetAxis("ChangeWeapon") < 0)
        {
            if (CurWeapon != 0)
                CurWeapon--;
        }*/
        #endregion

        EquipWeapon(CurWeapon);
    }
}
