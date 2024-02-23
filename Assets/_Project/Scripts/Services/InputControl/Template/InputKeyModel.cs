using UnityEngine.InputSystem;
using UnrealTeam.SB.Services.InputControl.Interfaces;

namespace UnrealTeam.SB.Services.InputControl.Template
{
    public class InputKeyModel : IKeyInputModel
    {
        private readonly InputAction _inputAction;
        
        public InputKeyModel(InputAction inputAction)
        {
            _inputAction = inputAction;
        }
        public bool IsPressed() => _inputAction.WasPressedThisFrame();
        public bool IsReleased() => _inputAction.WasReleasedThisFrame();
        public bool IsHold() => _inputAction.IsPressed();
    }
}