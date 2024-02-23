using UnityEngine.InputSystem;

namespace UnrealTeam.SB.Input
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