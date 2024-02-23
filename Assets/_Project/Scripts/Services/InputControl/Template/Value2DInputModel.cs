using UnityEngine;
using UnityEngine.InputSystem;
using UnrealTeam.SB.Services.InputControl.Interfaces;

namespace UnrealTeam.SB.Services.InputControl.Template
{
    public class Value2DInputModel : IValue2DInputModel
    {
        private readonly InputAction _inputAction;
        
        public Value2DInputModel(InputAction inputAction)
        {
            _inputAction = inputAction;
        }
        
        public bool IsPressed() => _inputAction.WasPressedThisFrame();
        public bool IsReleased() => _inputAction.WasReleasedThisFrame();
        public bool IsHold() => _inputAction.IsPressed();
        public Vector2 Value2D() => _inputAction.ReadValue<Vector2>();
    }
}