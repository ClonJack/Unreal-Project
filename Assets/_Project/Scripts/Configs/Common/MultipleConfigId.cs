using System;
using UnityEngine;

namespace UnrealTeam.SB.Configs.Common
{
    [Serializable]
    public class MultipleConfigId<TConfig>
        where TConfig : IMultipleConfig
    {
        [field: SerializeField] public string Id { get; private set; }
    }
}