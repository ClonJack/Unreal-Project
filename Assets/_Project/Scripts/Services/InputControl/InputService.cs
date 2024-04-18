using System;
using UnrealTeam.SB.Input;
using UnrealTeam.SB.Services.InputControl.Interfaces;
using UnrealTeam.SB.Services.InputControl.Template;

namespace UnrealTeam.SB.Services.InputControl
{
    public class InputService : IInputService, IDisposable
    {
        private readonly GameInput _gameInput;
        
        public IValueInputModel MoveAxisY { get; }
        public IValueInputModel MoveAxisX { get; }
        public IValue2DInputModel Look2DAxis { get; }
        public IValueInputModel Mouse { get; }
        public IValueInputModel Jump { get; }
        public IValueInputModel Use { get; }

        
        public InputService()
        {
            _gameInput = new GameInput();
            _gameInput.Enable();
            
            MoveAxisY = new InputValueModel(_gameInput.Player.AxisY);
            MoveAxisX = new InputValueModel(_gameInput.Player.AxisX);
            Look2DAxis = new Value2DInputModel(_gameInput.Player.Look);
            Mouse = new InputValueModel(_gameInput.Player.Mouse);
            Jump = new InputValueModel(_gameInput.Player.Jump);
            Use = new InputValueModel(_gameInput.Player.Use);
        }

        public void Dispose()
        {   
            _gameInput?.Disable();
        }
    }
}