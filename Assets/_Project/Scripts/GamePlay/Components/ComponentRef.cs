using UnityEngine;

namespace UnrealTeam.SB.Components
{
    public struct ComponentRef<T>
        where T : Component
    {
        public T Value;
    }
}