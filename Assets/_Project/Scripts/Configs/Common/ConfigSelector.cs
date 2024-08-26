using System;
using UnityEngine;

namespace UnrealTeam.SB.Configs.Common
{
    [Serializable]
    // ReSharper disable once UnusedTypeParameter
    public class ConfigSelector<TConfig>
        where TConfig : IMultipleConfig
    {
        [field: SerializeField] public string Id { get; private set; }
    }
}