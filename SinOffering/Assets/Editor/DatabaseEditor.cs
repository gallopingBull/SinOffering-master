//https://github.com/SunnyValleyStudio/unity_dictionary_visible_in_inspector_tutorial
//https://www.youtube.com/watch?v=OGnhLL4q_F8&t=146s
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Database))]
public class DatabaseEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (((Database)target).modifyValues)
        {
            if (GUILayout.Button("Save Changes"))
            {
                ((Database)target).DeserializeDictionary();
            }
        }
    }
}
