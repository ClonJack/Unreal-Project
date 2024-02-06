using UnityEngine;

namespace GamePlay.Components
{
    public struct ComponentRef<T>
        where T : Component
    {
        public T Value;
    }
}