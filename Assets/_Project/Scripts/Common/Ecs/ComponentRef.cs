using UnityEngine;

namespace UnrealTeam.SB.Common.Ecs
{
    public struct ComponentRef<T>
        where T : Component
    {
        public T Component;
    }
}