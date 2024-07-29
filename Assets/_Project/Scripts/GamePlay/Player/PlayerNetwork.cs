using Fusion;
using KinematicCharacterController;
using UnityEngine;
using UnrealTeam.SB.Common.Ecs.Binders;
using UnrealTeam.SB.GamePlay.CharacterController.Views;
using UnrealTeam.SB.Services.Other;
using VContainer;

namespace UnrealTeam.SB.GamePlay.Player
{
    public class PlayerNetwork : NetworkBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private KinematicCharacterMotor _characterMotor;
        [SerializeField] private CharacterView _characterView;
        [SerializeField] private EcsEntityProvider _ecsProvider;
        
        private ObjectsProvider _objectsProvider;


        [Inject]
        public void Construct(ObjectsProvider objectsProvider)
        {
            _objectsProvider = objectsProvider;
        }
        
        private void Start()
        {
            if (!HasInputAuthority) 
                return;
            
            _ecsProvider.CreateEntity();
            _ecsProvider.BuildComponents();
            
            _camera.gameObject.SetActive(true);
            _objectsProvider.GameCamera = _camera;
            
            _characterMotor.enabled = true;
            _characterView.enabled = true;
            
            _characterView.TeleportTo(transform);
        }
    }
}