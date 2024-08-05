using System;
using System.Linq;
using UnityEditor;
using Object = UnityEngine.Object;

namespace UnrealTeam.SB.Common.Utils
{
    public static class EditorUtils
    {
        public static void SetDirtyIfNot(Object obj)
        {
            if (!EditorUtility.IsDirty(obj))
                EditorUtility.SetDirty(obj);
        }

        public static void SaveAssets()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        public static void SaveSerialization(Object obj)
        {
            SetDirtyIfNot(obj);
            SaveAssets();
        }

        public static void SaveSerialization(SerializedObject serializedObject)
        {
            serializedObject.ApplyModifiedProperties();
            SaveAssets();
        }

        public static void SaveSerialization(SerializedProperty serializedProperty) 
            => SaveSerialization(serializedProperty.serializedObject);

        public static T[] GetSoInstances<T>() where T : Object
            => AssetDatabase
                .FindAssets($"t:{typeof(T).Name}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<T>)
                .ToArray();
        
        public static Object[] GetSoInstances(Type type)
            => AssetDatabase
                .FindAssets($"t:{type.Name}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(path => AssetDatabase.LoadAssetAtPath(path, type))
                .ToArray();
        
        public static TCastTo[] GetSoInstances<TCastTo>(Type type)
            => AssetDatabase
                .FindAssets($"t:{type.Name}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(path => AssetDatabase.LoadAssetAtPath(path, type))
                .Cast<TCastTo>()
                .ToArray();
    }
}