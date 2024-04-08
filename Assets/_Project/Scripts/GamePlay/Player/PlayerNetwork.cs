using Fusion;
using UnityEngine;
using UnrealTeam.SB.Common.Ecs.Binders;

namespace UnrealTeam.SB._Project.Scripts.GamePlay.Player
{
    public class PlayerNetwork : NetworkBehaviour
    {
        [SerializeField] private EcsEntityProvider _ecsEntityProvider;


        public override void Spawned()
        {
            if (HasInputAuthority)
            {
                _ecsEntityProvider.BuildEntity();
            }
        }
    }
}