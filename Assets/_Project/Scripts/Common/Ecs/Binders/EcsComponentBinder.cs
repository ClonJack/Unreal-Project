﻿using Leopotam.EcsLite;

namespace UnrealTeam.SB.Common.Ecs.Binders
{
    public abstract class EcsComponentBinder<T> : EcsBinderBase
        where T : struct
    {
        public sealed override void Init(int entity, EcsWorld ecsWorld)
        {
            ref var component = ref ecsWorld.GetPool<T>().Add(entity);
            InitData(ref component);
        }

        protected virtual void InitData(ref T component) {}
    }
}