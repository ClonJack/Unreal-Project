using UnityEngine;
using UnrealTeam.SB.Configs.Common;

namespace UnrealTeam.SB.Configs
{
    [CreateAssetMenu(menuName = "Configs/Mineral", fileName = "MineralConfig")]
    public class MineralConfig : SoMultipleConfig
    {
        [field: SerializeField] public string Name { get; private set; }
    }
}