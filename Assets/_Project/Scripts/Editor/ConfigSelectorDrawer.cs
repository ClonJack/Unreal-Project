using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnrealTeam.SB.Common.Utils;
using UnrealTeam.SB.Configs.Common;

namespace UnrealTeam.SB.Editor
{
    [CustomPropertyDrawer(typeof(ConfigSelector<>), true)]
    public class ConfigSelectorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var configType = fieldInfo.FieldType.GetGenericArguments()[0];
            if (!typeof(ScriptableObject).IsAssignableFrom(configType))
            {
                EditorGUI.LabelField(position, label.text, $"Config must be {nameof(ScriptableObject)}");
                return;
            }

            if (configType == typeof(IMultipleConfig) || configType == typeof(SoMultipleConfig))
            {
                EditorGUI.LabelField(position, label.text, "Select concrete config type");
                return;
            }
                
            var configs = EditorUtils.GetSoInstances<IMultipleConfig>(configType);
            if (configs == null || configs.Length == 0)
            {
                EditorGUI.LabelField(position, label.text, "No configs found");
                return;
            }

            var valueProp = property.FindPropertyRelative($"<{nameof(ConfigSelector<IMultipleConfig>.Id)}>k__BackingField");
            var currentId = valueProp.stringValue;
            var configNames = configs.Select(config => (config as ScriptableObject)?.name ?? "Unnamed").ToArray();
            var configIds = configs.Select(config => config.Id).ToArray();

            var currentIndex = Array.IndexOf(configIds, currentId);
            if (currentIndex < 0) currentIndex = 0;
                
            var selectedIndex = EditorGUI.Popup(position, label.text, currentIndex, configNames);
            if (selectedIndex >= 0 && selectedIndex < configIds.Length)
            {
                valueProp.stringValue = configIds[selectedIndex];
            }
        }
    }
}
