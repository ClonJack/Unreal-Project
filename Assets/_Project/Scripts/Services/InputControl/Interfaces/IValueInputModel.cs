namespace UnrealTeam.SB.Services.InputControl.Interfaces
{
    public interface IValueInputModel : IPressed, IReleased, IHold
    {
        float Value();
    }
}