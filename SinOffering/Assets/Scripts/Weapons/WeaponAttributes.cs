using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponAttributes
{
    //[HideInInspector]
    [SerializeField]
    public WeaponData weaponData;
    private Dictionary<string, WeaponData> weaponDatabase;

    //weapon variables

    [HideInInspector]
    public Weapon.WeaponTypes WeaponType;

    [HideInInspector]
    public string weaponName;


    public bool WeaponPurchased;

    [HideInInspector]
    public bool WeaponModififed = false;
    [HideInInspector]
    public bool SecondaryFire = false;

    [HideInInspector]
    public int MaxAmmo;

    public int AmmoCapacityLevel = 0;

    public float WeaponDamage;
    public int WeaponDamageLevel = 0;

    //[HideInInspector]
    public float fireRate = 0f;
    //[HideInInspector]
    public int fireRateLevel = 0;

    //Variables for weapon spread
    //public bool EnableSpread; //whether or not the weapon has any projectile spread
    [HideInInspector]
    public float MinSpread, MaxSpread;
    [HideInInspector]
    public int ProjectileSpreadLevel = 0; //so far used only for shotgun and gattling gun


    protected float RecoilAmmount;


    //intensity and duration of camera shake when gun
    //is firing
    public float DurationCamShake, AmmountCamShake;
    public bool SmoothTransition = false;


    public void InitWeaponAttributes()
    {
        //using an IDatabase
        //SetWeaponName(_weaponName);
        //GetWeaponAttributeData(_weaponName);
    }

    private void SetWeaponName(string _weaponName)
    {
        weaponName = _weaponName;
    }

    private void GetWeaponAttributeDataFromDatabase(string _weaponName)
    {
        //using an IDatabase
        weaponDatabase = Database._instance.GetWeaponDatabase();
        //Debug.Log("setting data for: " + _weaponName);
        weaponData = weaponDatabase[_weaponName];

    }


    public void SetUPWeaponAttribute(string _weaponName)
    {
        //Debug.Log("SetUpWeaponAttribute() - Weapon Name: " + _weaponName);
        SetWeaponName(_weaponName);
        GetWeaponAttributeDataFromDatabase(_weaponName);
    }

    public void SetWeaponAttributeData(WeaponAttributes _weaponData)
    {
        SetWeaponName(_weaponData.weaponName);
        GetWeaponAttributeDataFromDatabase(_weaponData.weaponName);


        fireRateLevel = _weaponData.fireRateLevel;
        SetFireRateValue(fireRateLevel);


        WeaponDamageLevel = _weaponData.WeaponDamageLevel;
        SetWeaponDamageValue(WeaponDamageLevel);


        AmmoCapacityLevel = _weaponData.AmmoCapacityLevel;
        SetAmmoCapacityValue(AmmoCapacityLevel);


        WeaponPurchased = _weaponData.WeaponPurchased;
        SecondaryFire = _weaponData.SecondaryFire;
    }
   

    private void SetFireRateValue(int _fireRateLevel)
    {
   
        for (int i = 0; i < weaponData.AttributeDataList.Length; i++)
        {
            if (weaponData.AttributeDataList[i].UpgradeType == WeaponUpgradeTypes.UpgradeType.FireRate && 
                _fireRateLevel == weaponData.AttributeDataList[i].AttributeLevel)
            {
                fireRate = weaponData.AttributeDataList[i].AttributeValue;
            }
        }
    }


    private void SetWeaponDamageValue(int _damageLevel)
    {
        for (int i = 0; i < weaponData.AttributeDataList.Length; i++)
        {
            if (weaponData.AttributeDataList[i].UpgradeType == WeaponUpgradeTypes.UpgradeType.WeaponDamage &&
                _damageLevel == weaponData.AttributeDataList[i].AttributeLevel)
            {
                WeaponDamage = weaponData.AttributeDataList[i].AttributeValue;
            }
        }
    }


    private void SetAmmoCapacityValue(int _ammoCapacityLevel)
    {
        for (int i = 0; i < weaponData.AttributeDataList.Length; i++)
        {
            if (weaponData.AttributeDataList[i].UpgradeType == WeaponUpgradeTypes.UpgradeType.AmmoCapacity &&
                _ammoCapacityLevel == weaponData.AttributeDataList[i].AttributeLevel)
            {
                MaxAmmo = (int)weaponData.AttributeDataList[i].AttributeValue;
            }
        }
    }

    public Dictionary<WeaponUpgradeTypes.UpgradeType, int> GetWeaponAttributeLevels()
    {
        Dictionary<WeaponUpgradeTypes.UpgradeType, int> tmpDic = new Dictionary<WeaponUpgradeTypes.UpgradeType, int>();

        tmpDic.Add(WeaponUpgradeTypes.UpgradeType.FireRate, fireRateLevel);
        tmpDic.Add(WeaponUpgradeTypes.UpgradeType.AmmoCapacity, AmmoCapacityLevel);
        tmpDic.Add(WeaponUpgradeTypes.UpgradeType.WeaponDamage, WeaponDamageLevel);
        return tmpDic;
    }
}

