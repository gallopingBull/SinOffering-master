//https://github.com/SunnyValleyStudio/unity_dictionary_visible_in_inspector_tutorial
//https://www.youtube.com/watch?v=OGnhLL4q_F8&t=146s

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Database_Attributes_SO)", menuName = "DataObjects/Database/Database_Attributes_SO")]

public class DatabaseScriptableObject_Attributes: ScriptableObject
{
    [SerializeField]
    private List<string> keys = new List<string>();
    
    //make this a generic type so this SO can be used for a varitey of types
    [SerializeField]
    private List<AttributeData> values = new List<AttributeData>();

    public List<string> Keys {   get => keys; set => keys = value; }
    public List<AttributeData> Values { get => values; set => values = value; }
}
