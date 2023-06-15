using UnityEditor;
using UnityEngine;

public class TreeDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TreeData treeData = (TreeData)target;

        EditorGUILayout.LabelField("Node Dictionary:");

        foreach (var kvp in treeData.nodeDictionary)
        {
            EditorGUILayout.LabelField(kvp.Key, kvp.Value.ToString());
        }

        DrawDefaultInspector();
    }
}
