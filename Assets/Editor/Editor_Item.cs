using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemInfo))]
public class Editor_Item : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ItemInfo generator = (ItemInfo)target;
        if (GUILayout.Button("Set Info"))
        {
            generator.SetInfo();
        }
    }
}
