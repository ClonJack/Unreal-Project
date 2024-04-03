using Leopotam.EcsLite;

namespace UnrealTeam.SB.Common.Ecs.Providers
{
    public abstract class EcsComponentProvider<T> : EcsProvider 
        where T : struct
    {
        public sealed override void Init(int entity, EcsWorld ecsWorld)
        {
            ref T component = ref ecsWorld.GetPool<T>().Add(entity);
            InitData(ref component);
        }

        protected virtual void InitData(ref T component) {}
    }
}