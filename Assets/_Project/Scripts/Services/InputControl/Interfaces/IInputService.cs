using UnrealTeam.SB.Input;

namespace UnrealTeam.SB.Services.InputControl.Interfaces
{
    public interface IInputService
    {
        public GameInput GameInput { get; set; }
        public IValueInputModel MoveAxisY { get; set; }
        public IValueInputModel MoveAxisX { get; set; }
        public IValue2DInputModel Look2DAxis { get; set; }
        public IValueInputModel Mouse { get; set; }
        public IValueInputModel Jump { get; set; }
    }
}