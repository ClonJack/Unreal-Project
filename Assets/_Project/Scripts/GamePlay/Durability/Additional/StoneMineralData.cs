using System;
using UnityEngine;
using UnrealTeam.SB.Configs;
using UnrealTeam.SB.Configs.Common;

namespace UnrealTeam.SB.GamePlay.Durability.Additional
{
    [Serializable]
    public class StoneMineralData
    {
        [SerializeField] private ConfigSelector<MineralConfig> _mineral;

        [field: SerializeField] public float MinWeights { get; private set; }
        [field: SerializeField] public float MaxWeights { get; private set; }
        
        public string MineralId => _mineral.Id;
    }
}