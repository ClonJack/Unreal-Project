using UnityEditor;
using UnityEngine;
using UnrealTeam.SB.Additional.Game;

namespace UnrealTeam.SB.Editor
{
    [CustomEditor(typeof(LocalizeTMP))]
    public class LocalizeTMPEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_textMesh"), new GUIContent("TextMeshPro Component"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_StringReference"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_FormatArguments"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}