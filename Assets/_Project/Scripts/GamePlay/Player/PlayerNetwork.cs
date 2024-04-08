using Fusion;
using UnityEngine;
using UnrealTeam.SB.Common.Ecs.Binders;
using VContainer;

namespace UnrealTeam.SB._Project.Scripts.GamePlay.Player
{
    public class PlayerNetwork : NetworkBehaviour
    {
        [SerializeField] private EcsEntityProvider _ecsEntityProvider;
        [SerializeField] private Camera _camera;

        [Inject]
        public void Construct()
        {
            if (HasInputAuthority)
            {
                _ecsEntityProvider.BuildEntity();

                _camera.gameObject.SetActive(true);
            }
        }
    }
}