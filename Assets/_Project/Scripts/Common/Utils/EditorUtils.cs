using UnityEditor;
using UnityEngine;

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
    }
}