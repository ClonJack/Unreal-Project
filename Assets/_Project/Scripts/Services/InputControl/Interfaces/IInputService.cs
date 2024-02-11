namespace UnrealTeam.SB.Input
{
    public interface IInputService
    {
        public IValueInputModel MoveAxisY { get; set; }
        public IValueInputModel MoveAxisX { get; set; }
        public IValue2DInputModel Look2DAxis { get; set; }
    }
}