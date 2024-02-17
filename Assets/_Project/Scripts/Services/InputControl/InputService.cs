using System;

namespace UnrealTeam.SB.Input
{
    public class InputService : IInputService, IDisposable
    {
        private GameInput _gameInput = null;
        public IValueInputModel MoveAxisY { get; set; }
        public IValueInputModel MoveAxisX { get; set; }
        public IValue2DInputModel Look2DAxis { get; set; }
        public IValueInputModel Mouse { get; set; }

        public InputService()
        {
            _gameInput = new GameInput();

            _gameInput.Enable();

            MoveAxisY = new InputValueModel(_gameInput.Player.AxisY);
            MoveAxisX = new InputValueModel(_gameInput.Player.AxisX);
            Look2DAxis = new Value2DInputModel(_gameInput.Player.Look);
            Mouse = new InputValueModel(_gameInput.Player.Mouse);
        }

        public void Dispose()
        {
            _gameInput?.Disable();
            _gameInput?.Dispose();
        }
    }
}