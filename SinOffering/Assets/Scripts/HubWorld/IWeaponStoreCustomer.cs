using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//player uses this interface
public interface IWeaponStoreCustomer 
{
    void PurchaseWeapon(string _weaponName);
    void PurchaseWeaponUpgrade(string _weaponName, WeaponUpgradeTypes.UpgradeType upgradeType);

    bool CanPurchaseWeapon(int _price);
    bool CanPurchaseWeaponUpgrade(int _price);
}

