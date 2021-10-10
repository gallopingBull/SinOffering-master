using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttributeStoreCustomer
{
    void PurchaseUpgrade(string _upgradeType); //change this attributeupgradetype

    bool CanPurchaseUpgrade(int _price);

}
