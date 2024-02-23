using UnityEngine;

namespace UnrealTeam.SB.Input
{
    public interface IValue2DInputModel : IPressed, IReleased, IHold
    {
        public Vector2 Value2D();
    }
}