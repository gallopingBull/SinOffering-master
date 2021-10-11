using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttributeStoreCustomer
{
    void PurchaseUpgrade(AttributeUpgradeTypes.UpgradeType _upgradeType); //change this attributeupgradetype

    bool CanPurchaseUpgrade(int _price);

}
