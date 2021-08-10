using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Maybe change the name of this class to WeaponInventory.cs
public class WeaponManager : MonoBehaviour {

    public GameObject[] Weapons;
    public int CurWeapon;
    public Transform LHandSocket, RHandSocket; 

    private PlayerController parent; 

    // Use this for initialization
    void Awake () { parent = GetComponent<PlayerController>(); }

    private void Start()
    {
        Invoke("InitWeapons", .2f);    
    }

    //give every weapon a default "temp" weaponattribute 
    //if one isn't read in during the load process
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
        if (parent.EquippedWeapon != null)
        {
            parent.ResetSpeed(); //reset player to default speed
            parent.EquippedWeapon.SetActive(false);
        }
        
        CurWeapon = newWeapon;

        parent.EquippedWeapon = Weapons[CurWeapon];
        parent.EquippedWeapon.SetActive(true);

        parent.EquippedWeapon.GetComponent<Weapon>().InitWeapon(); 
        //have the weapon call InitWeapon()
        //wihtin itself instead
    }

    public void ChangeWeapon()
    {
        if (Input.GetAxis("ChangeWeapon") > 0)
        {
            if (CurWeapon != Weapons.Length-1)
            {
                CurWeapon++;
            }
        }
        if (Input.GetAxis("ChangeWeapon") < 0)
        {
            if (CurWeapon != 0)
            {
                CurWeapon--;
            }
        }
        EquipWeapon(CurWeapon);
    }
}
