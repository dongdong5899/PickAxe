using UnityEditor;
using UnityEditorInternal;
using UnityEditorInternal.VR;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(StatCompo))]
public class StatCustomInspector : Editor
{
    SerializedProperty statProp;

    private void OnEnable()
    {
        statProp = serializedObject.FindProperty("_stat");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        //EditorGUILayout.PropertyField(statProp);

        if (statProp.objectReferenceValue != null)
        {
            EditorGUI.indentLevel++;
            SerializedObject so = new SerializedObject(statProp.objectReferenceValue);
            so.Update();

            SerializedProperty prop = so.GetIterator();
            prop.NextVisible(true);
            while (prop.NextVisible(false))
            {
                EditorGUILayout.PropertyField(prop, true);
            }

            so.ApplyModifiedProperties();
            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
