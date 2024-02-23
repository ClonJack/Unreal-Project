namespace UnrealTeam.SB.Input
{
    public interface IValueInputModel : IPressed, IReleased, IHold
    {
        float Value();
    }
}