using System;
using UnityEngine;
using UnrealTeam.SB.Configs;
using UnrealTeam.SB.Configs.Common;

namespace UnrealTeam.SB.GamePlay.Durability.Additional
{
    [Serializable]
    public class MineralWeightData
    {
        [field: SerializeField] public float MinWeight { get; private set; }
        [field: SerializeField] public float MaxWeight { get; private set; }
        [field: SerializeField] public MultipleConfigId<MineralConfig> Mineral { get; private set; }
    }
}