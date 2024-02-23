using UnityEngine;

namespace UnrealTeam.SB.Services.InputControl.Interfaces
{
    public interface IValue2DInputModel : IPressed, IReleased, IHold
    {
        public Vector2 Value2D();
    }
}