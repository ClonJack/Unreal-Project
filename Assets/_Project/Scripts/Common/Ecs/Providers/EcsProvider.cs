using System;
using Leopotam.EcsLite;

namespace UnrealTeam.SB.Common.Ecs.Providers
{
    [Serializable]
    public abstract class EcsProvider
    {
        public abstract void Init(int entity, EcsWorld ecsWorld);
    }
}