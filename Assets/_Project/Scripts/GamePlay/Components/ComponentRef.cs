using UnityEngine;

namespace UnrealTeam.SB.GamePlay.Components
{
    public struct ComponentRef<T>
        where T : Component
    {
        public T Value;
    }
}