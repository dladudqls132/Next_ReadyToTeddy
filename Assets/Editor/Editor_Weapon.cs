using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WeaponInfo))]
public class Editor_Weapon : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        WeaponInfo generator = (WeaponInfo)target;
        if (GUILayout.Button("Set Info"))
        {
            generator.SetInfo();
        }
    }
}