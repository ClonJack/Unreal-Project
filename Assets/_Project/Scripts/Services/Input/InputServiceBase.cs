using Common.Constants;
using UnityEngine;

namespace Services.Input
{
    public abstract class InputServiceBase : IInputService
    {
        public Vector2 GetMoveDirection()
            => new Vector2(
                UnityEngine.Input.GetAxisRaw(InputConstants.Horizontal),
                UnityEngine.Input.GetAxisRaw(InputConstants.Vertical)).normalized;
    }
}