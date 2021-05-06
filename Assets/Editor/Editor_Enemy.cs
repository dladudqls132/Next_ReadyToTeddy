using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyInfo))] 
public class Editor_Enemy : Editor 
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EnemyInfo generator = (EnemyInfo)target;
        if (GUILayout.Button("Set Info"))
        {
            generator.SetInfo();
        }
    }
}
