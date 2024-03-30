using System;
using Leopotam.EcsLite;

namespace UnrealTeam.SB.Common.Ecs.Providers
{
    [Serializable]
    public abstract class EcsComponentProviderBase
    {
        public virtual void AddComponent(int entity, EcsWorld ecsWorld){}
    }
}