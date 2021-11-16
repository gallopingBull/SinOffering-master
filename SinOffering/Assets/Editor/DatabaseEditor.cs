//https://github.com/SunnyValleyStudio/unity_dictionary_visible_in_inspector_tutorial
//https://www.youtube.com/watch?v=OGnhLL4q_F8&t=146s
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(WeaponDatabase))]
public class DatabaseEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (((WeaponDatabase)target).modifyValues)
        {
            if (GUILayout.Button("Save Changes"))
            {
                ((WeaponDatabase)target).DeserializeDictionary();
            }
        }
    }
}
[CustomEditor(typeof(OfferingDatabase))]

public class AttritubteDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (((OfferingDatabase)target).modifyValues)
        {
            if (GUILayout.Button("Save Changes"))
            {
                ((OfferingDatabase)target).DeserializeDictionary();
            }
        }
    }
}
