using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//IDatabase
public interface IInventory
{
    WeaponData GetWeaponData(int weaponID);
    void GetAbilityData(int abilityID);  
}
