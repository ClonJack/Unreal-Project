using System;
using Leopotam.EcsLite;

namespace UnrealTeam.SB.Common.Ecs.Binders
{
    [Serializable]
    public abstract class EcsBinderBase
    {
        public abstract void Init(int entity, EcsWorld ecsWorld);
    }
}