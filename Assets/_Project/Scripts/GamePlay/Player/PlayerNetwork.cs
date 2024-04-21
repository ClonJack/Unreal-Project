using Fusion;
using KinematicCharacterController;
using UnityEngine;
using UnrealTeam.SB.Common.Ecs.Binders;
using UnrealTeam.SB.GamePlay.CharacterController.Views;

namespace UnrealTeam.SB.GamePlay.Player
{
    public class PlayerNetwork : NetworkBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private KinematicCharacterMotor _characterMotor;
        [SerializeField] private CharacterView _characterView;
        [SerializeField] private EcsEntityProvider _ecsProvider;

        private void Start()
        {
            if (!HasInputAuthority) 
                return;
            
            _ecsProvider.CreateEntity();
            _ecsProvider.BuildComponents();
            _camera.gameObject.SetActive(true);
            _characterMotor.enabled = true;
            _characterView.enabled = true;
        }
    }
}