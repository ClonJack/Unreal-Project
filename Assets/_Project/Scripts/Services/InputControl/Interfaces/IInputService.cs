using UnrealTeam.SB.Input;

namespace UnrealTeam.SB.Services.InputControl.Interfaces
{
    public interface IInputService
    {
        public IValueInputModel MoveAxisY { get; }
        public IValueInputModel MoveAxisX { get; }
        public IValue2DInputModel Look2DAxis { get; }
        public IValueInputModel Mouse { get; }
        public IValueInputModel Jump { get; }
        public IValueInputModel Use { get; }
    }
}