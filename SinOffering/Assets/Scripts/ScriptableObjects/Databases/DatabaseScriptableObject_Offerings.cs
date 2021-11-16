//https://github.com/SunnyValleyStudio/unity_dictionary_visible_in_inspector_tutorial
//https://www.youtube.com/watch?v=OGnhLL4q_F8&t=146s

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Database_Offerings_SO", menuName = "DataObjects/Database/Database_Offerings_SO")]

public class DatabaseScriptableObject_Offerings: ScriptableObject
{
    [SerializeField]
    private List<int> keys = new List<int>();
    
    //make this a generic type so this SO can be used for a varitey of types
    [SerializeField]
    private List<OfferingData> values = new List<OfferingData>();

    public List<int> Keys {   get => keys; set => keys = value; }
    public List<OfferingData> Values { get => values; set => values = value; }
}
