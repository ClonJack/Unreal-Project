using UnityEngine;
using UnityEngine.AddressableAssets;
using UnrealTeam.SB.Configs.Common;

namespace UnrealTeam.SB.Configs.Player
{
    [CreateAssetMenu(menuName = "Configs/Player", fileName = "PlayerConfig")]
    public class PlayerConfig : ScriptableObject, ISingleConfig
    {
        [field: SerializeField] public AssetReferenceT<GameObject> PrefabCharacter { get; private set; }
        [field: SerializeField] public AssetReferenceT<GameObject> PrefabCamera { get; private set; }
        [field: SerializeField] public AssetReferenceT<GameObject> PlayerPrefab { get; private set; }
    }
}