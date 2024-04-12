using UnityEngine;

namespace UnrealTeam.SB.Configs.AI
{
    [CreateAssetMenu(menuName = "Configs/AI/NavMesh Agent")]
    public class NavMeshAgentConfig : ScriptableObject, IMoveConfig
    {
        [field: SerializeField, Min(0)]
        public float MoveSpeed { get; private set; } = 3.5f;

        [field: SerializeField, Min(0), Tooltip("Degrees in second")]
        public float RotationSpeed { get; private set; } = 120;

        [field: SerializeField, Min(0)]
        public float StoppingDistance { get; private set; } = 0.5f;
    }
}