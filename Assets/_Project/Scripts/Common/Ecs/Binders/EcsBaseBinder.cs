using System;
using Leopotam.EcsLite;

namespace UnrealTeam.SB.Common.Ecs.Binders
{
    [Serializable]
    public abstract class EcsBaseBinder
    {
        public abstract void Init(int entity, EcsWorld ecsWorld);
    }
}