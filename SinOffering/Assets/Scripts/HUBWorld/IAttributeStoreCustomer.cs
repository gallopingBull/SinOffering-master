using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttributeStoreCustomer
{
    void PurchaseUpgrade(string _upgradeName);

    bool CanPurchaseUpgrade(int _price);

}
