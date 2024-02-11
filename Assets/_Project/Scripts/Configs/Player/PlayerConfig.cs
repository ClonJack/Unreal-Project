using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UnrealTeam.SB.Configs
{
    [CreateAssetMenu(menuName = "Configs/Player", fileName = "PlayerConfig")]
    public class PlayerConfig : ScriptableObject, ISingleConfig
    {
        [field: SerializeField] public AssetReferenceT<GameObject> Prefab { get; private set; }
        [field: SerializeField] public float MoveSpeed { get; private set; }
        [field: SerializeField] public float RotationSpeed { get; private set; }
    }
}