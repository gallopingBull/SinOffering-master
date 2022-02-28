using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Maybe change the name of this class to WeaponInventory.cs
public class WeaponManager : MonoBehaviour {
    public int CurWeapon;
    public bool WeaponEquipped = false;
    public GameObject[] Weapons;
    
    public Transform mainHandSocket;
    [SerializeField]
    private float handSocketOffsetValue = 1.25f;
    private PlayerController pc; 

    // Use this for initialization
    void Awake () { pc = GetComponent<PlayerController>(); }

    private void Start()
    {
        Invoke("InitWeapons", .2f);    
    }

    private void Update()
    {
        if (pc.EquippedWeapon != null)
            UpdateWeaponSocket();
     
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
        Vector3 tmpPos = mainHandSocket.localPosition;
        if (pc.inputHandler.aiming)
        {
            if (pc.inputHandler._aimDir != 1)
            {
                Debug.Log("calling UpdateWeaponSocket() from aiming condition");
              
                tmpPos.x *= -handSocketOffsetValue;

            }
            
        }
        else
        {
            if (pc.dir != 1)
            {
                Debug.Log("calling UpdateWeaponSocket() from non-aiming condition");


                tmpPos.x *= -handSocketOffsetValue;
           
            }
        }

        mainHandSocket.localPosition = tmpPos;
        pc.EquippedWeapon.transform.position = mainHandSocket.position;

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
        if (CurWeapon != Weapons.Length - 1)
            CurWeapon++;
        else
            CurWeapon = 0;
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
