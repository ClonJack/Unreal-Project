using Fusion;
using Leopotam.EcsLite;
using UnityEngine;
using UnrealTeam.SB.Common.Ecs.Binders;
using VContainer;

namespace UnrealTeam.SB._Project.Scripts.GamePlay.Player
{
    public class PlayerNetwork : NetworkBehaviour
    {
        [SerializeField] private EcsEntityProvider _ecsEntityProvider;

        private EcsWorld _ecsWorld;
        
        [Inject]
        public void Construct(EcsWorld ecsWorld)
        {
            _ecsWorld = ecsWorld;

            if (HasInputAuthority)
            {
                _ecsEntityProvider.BuildEntity();
            }

            //  BuildEntity();
        }
        public override void Spawned()
        {
            Debug.Log("Spawned");
            
          /*  if (HasInputAuthority)
            {
                _ecsEntityProvider.BuildEntity(_ecsWorld);
            }*/
        }
    }
}