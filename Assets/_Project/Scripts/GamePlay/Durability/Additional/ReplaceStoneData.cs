using System;
using UnityEngine;

namespace UnrealTeam.SB.GamePlay.Durability.Additional
{
    [Serializable]
    public class ReplaceStoneData
    {
        [field: SerializeField] public int PiecesCount { get; private set; }
        [field: SerializeField] public GameObject[] PrefabVariants { get; private set; }
    }
}