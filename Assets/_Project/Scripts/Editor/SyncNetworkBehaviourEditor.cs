using UnityEditor;
using System.Reflection;
using System.Linq;
using Fusion;
using UnrealTeam.SB.Common.Utils;
using UnrealTeam.SB.GamePlay.Common.Views;

namespace UnrealTeam.SB.Editor
{
    [CustomEditor(typeof(SyncNetworkBehaviour), true)]
    public class SyncNetworkBehaviourEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawFields();
            UpdateHasNetworked();
        }

        private void DrawFields()
        {
            serializedObject.Update();
            var property = serializedObject.GetIterator();
            var enterChildren = true;
            while (property.NextVisible(enterChildren))
            {
                if (property.propertyPath == "m_Script")
                {
                    enterChildren = false;
                    continue;
                }

                if (typeof(SyncNetworkBehaviour).GetField(property.name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance) != null)
                {
                    enterChildren = false;
                    continue;
                }

                EditorGUILayout.PropertyField(property, true);
                enterChildren = false;
            }

            property.Reset();
            enterChildren = true;
            while (property.NextVisible(enterChildren))
            {
                if (property.propertyPath == "m_Script" || !(typeof(SyncNetworkBehaviour).GetField(property.name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance) != null))
                {
                    enterChildren = false;
                    continue;
                }

                EditorGUILayout.PropertyField(property, true);
                enterChildren = false;
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void UpdateHasNetworked()
        {
            var myTarget = (SyncNetworkBehaviour)target;
            var properties = myTarget.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            var hasNetworkedFields = properties.Any(prop => prop.GetCustomAttribute<NetworkedAttribute>() != null);

            FieldInfo hasNetworkedFieldInfo = null;
            var currentType = myTarget.GetType();

            while (currentType != null && hasNetworkedFieldInfo == null)
            {
                hasNetworkedFieldInfo = currentType.GetField("_hasNetworkedFields", BindingFlags.Instance | BindingFlags.NonPublic);
                currentType = currentType.BaseType;
            }

            if (hasNetworkedFieldInfo != null)
            {
                if ((bool)hasNetworkedFieldInfo.GetValue(myTarget) == hasNetworkedFields)
                    return;

                hasNetworkedFieldInfo.SetValue(myTarget, hasNetworkedFields);
                EditorUtils.SetDirtyIfNot(myTarget);
            }
        }
    }
}