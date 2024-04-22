using Leopotam.EcsLite;

namespace UnrealTeam.SB.Common.Ecs.Extensions
{
    public static class EcsPoolExtensions
    {
        public static ref T Replace<T>(this EcsPool<T> pool, int entity) where T : struct
        {
            if (pool.Has(entity))
                pool.Del(entity);

            return ref pool.Add(entity);
        }        
        
        public static void SafeDel<T>(this EcsPool<T> pool, int entity) 
            where T : struct
        {
            if (pool.Has(entity))
                pool.Del(entity);
        }

        public static ref T GetOrAdd<T>(this EcsPool<T> pool, int entity) where T : struct
        {
            if (!pool.Has(entity))
                return ref pool.Add(entity);

            return ref pool.Get(entity);
        }
    }
}