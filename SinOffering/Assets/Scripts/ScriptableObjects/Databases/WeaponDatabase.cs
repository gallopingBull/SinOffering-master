//https://github.com/SunnyValleyStudio/unity_dictionary_visible_in_inspector_tutorial
//https://www.youtube.com/watch?v=OGnhLL4q_F8&t=146s

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// WeaponDatabase
// work around to expose both keys/values inside a dictionary in the unity inspector

/// </CHANGES NEEDED>
/// 
/// make database a generic class that stores data 
/// that contains a reference to either of these data types: weaponData, 
/// attributeData, maybe abilityData... 
/// 
/// </summary>

public class WeaponDatabase: MonoBehaviour, ISerializationCallbackReceiver
{
    public bool modifyValues;
    public static WeaponDatabase _instance;

    //WeaponDatabaseSO
    [SerializeField]
    private DatabaseScriptableObject inventoryData;

    [SerializeField]
    private List<string> keys = new List<string>(); // list to to "expose" keys for dictionary in editor

    [SerializeField]
    private  List<WeaponData> values = new List<WeaponData>(); // list to to "expose" values for dictionary in editor

    [SerializeReference]
    private Dictionary<string, WeaponData> weapons = new Dictionary<string, WeaponData>();

    private void Awake()
    {
        _instance = this;
    }

    public void OnBeforeSerialize()
    {
        if (!modifyValues)
        {
            keys.Clear();
            values.Clear();

            for (int i = 0; i < Mathf.Min(inventoryData.Keys.Count, inventoryData.Values.Count); i++)
            {
                keys.Add(inventoryData.Keys[i]);
                values.Add(inventoryData.Values[i]);
            }
        }
    }

    public void OnAfterDeserialize()
    {
        // keep empty
    }

    // this is used to fuck with keys/values in the inspector
    public void DeserializeDictionary()
    {
        weapons = new Dictionary<string, WeaponData>();
        inventoryData.Keys.Clear();
        inventoryData.Values.Clear();

        for (int i = 0; i < Mathf.Min(keys.Count, values.Count); i++)
        {
            inventoryData.Keys.Add(keys[i]);
            inventoryData.Values.Add(values[i]);
            weapons.Add(keys[i], values[i]);
        }
        modifyValues = false;
    }

    // this is used to get dictionary at runtime
    public Dictionary<string, WeaponData> GetWeaponDatabase()
    {
        weapons = new Dictionary<string, WeaponData>();

        inventoryData.Keys.Clear();
        inventoryData.Values.Clear();

        for (int i = 0; i < Mathf.Min(keys.Count, values.Count); i++)
        {
            inventoryData.Keys.Add(keys[i]);
            inventoryData.Values.Add(values[i]);
            weapons.Add(keys[i], values[i]);
        }
        return weapons;
    }
}

[System.Serializable]
public class UpgradeDataValues   
{   
    public int AttributePrice;
    public float AttributeValue;
    public int AttributeLevel;
    public bool isPurchased;
}

