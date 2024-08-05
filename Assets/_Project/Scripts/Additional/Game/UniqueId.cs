using System;
using UnityEngine;

namespace UnrealTeam.SB.Additional.Game
{
    [Serializable]
    public class UniqueId
    {
        [field: SerializeField] public string Value { get; set; }
    }
}