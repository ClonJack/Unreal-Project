using UnityEngine;
using UnrealTeam.SB.Additional.Game;

namespace UnrealTeam.SB.Configs.Common
{
    public abstract class SoMultipleConfig : ScriptableObject, IMultipleConfig
    {
        [SerializeField] private UniqueId _uniqueId = new();
        
        public string Id => _uniqueId.Value;
    }
}