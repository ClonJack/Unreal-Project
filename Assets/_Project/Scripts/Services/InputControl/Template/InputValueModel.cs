using UnityEngine.InputSystem;
using UnrealTeam.SB.Services.InputControl.Interfaces;

namespace UnrealTeam.SB.Services.InputControl.Template
{
    public class InputValueModel : IValueInputModel
    {
        private readonly InputAction _inputAction;

        public InputValueModel(InputAction inputAction)
        {
            _inputAction = inputAction;
        }
        public bool IsPressed() => _inputAction.WasPressedThisFrame();
        public bool IsReleased() => _inputAction.WasReleasedThisFrame();
        public bool IsHold() => _inputAction.IsPressed();
        public float Value() => _inputAction.ReadValue<float>();
    }
}