using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UnrealTeam.SB.Configs
{
    [CreateAssetMenu(menuName = "Configs/Player", fileName = "PlayerConfig")]
    public class PlayerConfig : ScriptableObject, ISingleConfig
    {
        [field: SerializeField] public AssetReferenceT<GameObject> PrefabCharacter { get; private set; }
        [field: SerializeField] public AssetReferenceT<GameObject> PrefabCamera { get; private set; }
    }
}