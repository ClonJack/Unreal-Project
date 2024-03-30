using Leopotam.EcsLite;

namespace UnrealTeam.SB.Common.Ecs.Providers
{
    public abstract class EcsComponentProvider<T> : EcsComponentProviderBase 
        where T : struct
    {
        public sealed override void AddComponent(int entity, EcsWorld ecsWorld)
        {
            ref T component = ref ecsWorld.GetPool<T>().Add(entity);
            InitComponent(ref component);
        }

        protected virtual void InitComponent(ref T component) {}
    }
}