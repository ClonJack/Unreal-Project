using System;
using UnrealTeam.SB.Input;
using UnrealTeam.SB.Services.InputControl.Interfaces;
using UnrealTeam.SB.Services.InputControl.Template;

namespace UnrealTeam.SB.Services.InputControl
{
    public class InputService : IInputService, IDisposable
    {
        public GameInput GameInput { get; set; }
        public IValueInputModel MoveAxisY { get; set; }
        public IValueInputModel MoveAxisX { get; set; }
        public IValue2DInputModel Look2DAxis { get; set; }
        public IValueInputModel Mouse { get; set; }
        public IValueInputModel Jump { get; set; }

        public InputService()
        {
            GameInput = new GameInput();

            GameInput.Enable();

            MoveAxisY = new InputValueModel(GameInput.Player.AxisY);
            MoveAxisX = new InputValueModel(GameInput.Player.AxisX);
            Look2DAxis = new Value2DInputModel(GameInput.Player.Look);
            Mouse = new InputValueModel(GameInput.Player.Mouse);
            Jump = new InputValueModel(GameInput.Player.Jump);
        }

        public void Dispose()
        {
            GameInput?.Disable();
            GameInput?.Dispose();
        }
    }
}