//https://github.com/SunnyValleyStudio/unity_dictionary_visible_in_inspector_tutorial
//https://www.youtube.com/watch?v=OGnhLL4q_F8&t=146s

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// OfferingDatabase
// work around to expose both keys/values inside a dictionary in the unity inspector

public class OfferingDatabase: MonoBehaviour, ISerializationCallbackReceiver
{
    public bool modifyValues;
    public static OfferingDatabase _instance;

    // Database SO
    [SerializeField]
    private DatabaseScriptableObject_Offerings offeringData;

    [SerializeField]
    private List<int> keys = new List<int>(); // list to to "expose" keys for dictionary in editor

    [SerializeField]
    private  List<OfferingData> values = new List<OfferingData>(); //list to to "expose" values for dictionary in editor


    [SerializeReference]
    private Dictionary<int,  OfferingData> _offerings = new Dictionary<int, OfferingData>();

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

            for (int i = 0; i < Mathf.Min(offeringData.Keys.Count, offeringData.Values.Count); i++)
            {
                keys.Add(offeringData.Keys[i]);
                values.Add(offeringData.Values[i]);
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
        _offerings = new Dictionary<int, OfferingData>();

        offeringData.Keys.Clear();
        offeringData.Values.Clear();

        for (int i = 0; i < Mathf.Min(keys.Count, values.Count); i++)
        {
            offeringData.Keys.Add(keys[i]);
            offeringData.Values.Add(values[i]);
            _offerings.Add(keys[i], values[i]);
        }
        modifyValues = false;
    }

    // this is used to get dictionary at runtime
    public Dictionary<int, OfferingData> GetOfferingDatabase()
    {
        _offerings = new Dictionary<int, OfferingData>();

        offeringData.Keys.Clear();
        offeringData.Values.Clear();

        for (int i = 0; i < Mathf.Min(keys.Count, values.Count); i++)
        {
            offeringData.Keys.Add(keys[i]);
            offeringData.Values.Add(values[i]);
            _offerings.Add(keys[i], values[i]);
        }
        return _offerings;
    }
}


