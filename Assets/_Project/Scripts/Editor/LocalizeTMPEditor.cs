using UnityEditor;
using UnityEngine;
using UnrealTeam.SB.Common.Game;

namespace UnrealTeam.SB._Project.Scripts.Editor
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