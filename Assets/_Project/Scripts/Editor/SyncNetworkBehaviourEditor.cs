using System.Linq;
using System.Reflection;
using Fusion;
using UnityEditor;
using UnrealTeam.SB.Common.Utils;
using UnrealTeam.SB.GamePlay.Common.Views;

namespace UnrealTeam.SB.Editor
{
    [CustomEditor(typeof(SyncNetworkBehaviour), true)]
    public class SyncNetworkBehaviourEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
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