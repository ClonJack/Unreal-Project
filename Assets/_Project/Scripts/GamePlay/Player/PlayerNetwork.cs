using Fusion;
using KinematicCharacterController;
using UnityEngine;
using UnrealTeam.SB.GamePlay.CharacterController.Views;
using VContainer;

namespace UnrealTeam.SB.GamePlay.Player
{
    public class PlayerNetwork : NetworkBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private KinematicCharacterMotor _characterMotor;
        [SerializeField] private CharacterView _characterView;

        
        [Inject] 
        public void Construct()
        {
            if (HasInputAuthority)
            {
                _camera.gameObject.SetActive(true);
                _characterMotor.enabled = true;
                _characterView.enabled = true;
            }
        }
    }
}