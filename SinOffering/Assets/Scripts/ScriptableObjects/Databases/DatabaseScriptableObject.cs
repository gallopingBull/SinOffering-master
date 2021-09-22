//https://github.com/SunnyValleyStudio/unity_dictionary_visible_in_inspector_tutorial
//https://www.youtube.com/watch?v=OGnhLL4q_F8&t=146s

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Database", menuName = "DataObjects/Database")]

public class DatabaseScriptableObject : ScriptableObject
{
    [SerializeField]
    private List<string> keys = new List<string>();


    //make this a generic type so this SO can be used for a varitey of types
    [SerializeField]
    private List<WeaponData> values = new List<WeaponData>();

    public List<string> Keys {   get => keys; set => keys = value; }
    public List<WeaponData> Values { get => values; set => values = value; }
}
