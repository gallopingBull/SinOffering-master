//https://github.com/SunnyValleyStudio/unity_dictionary_visible_in_inspector_tutorial
//https://www.youtube.com/watch?v=OGnhLL4q_F8&t=146s

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// WeaponDatabase
// work around to expose both keys/values inside a dictionary in the unity inspector

public class AttributeDatabase: MonoBehaviour, ISerializationCallbackReceiver
{
    public bool modifyValues;
    public static AttributeDatabase _instance;

    //WeaponDatabaseSO
    [SerializeField]
    private DatabaseScriptableObject_Attributes inventoryData;

    [SerializeField]
    private List<string> keys = new List<string>(); // list to to "expose" keys for dictionary in editor

    [SerializeField]
    private  List<AttributeData> values = new List<AttributeData>(); //list to to "expose" values for dictionary in editor


    [SerializeReference]
    private Dictionary<string, AttributeData> weapons = new Dictionary<string, AttributeData>();

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
        weapons = new Dictionary<string, AttributeData>();

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
    public Dictionary<string, AttributeData> GetWeaponDatabase()
    {
        weapons = new Dictionary<string, AttributeData>();

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


