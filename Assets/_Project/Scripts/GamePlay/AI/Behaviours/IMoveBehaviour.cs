using UnityEngine;
using UnrealTeam.SB.Configs.AI;

namespace UnrealTeam.SB.GamePlay.AI.Behaviours
{
    public interface IMoveBehaviour
    {
        public bool TargetReached { get; }
        public bool IsValidPosition(Vector3 targetPosition, float maxDistance, out Vector3 positionToMove);
        public void MoveTo(Vector3 positionToMove);
        public void StopMoving();
        public void SetParams(IMoveConfig moveConfig);
        public void ResetParams();
    }
}