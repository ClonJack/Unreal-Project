using Leopotam.EcsLite;

namespace UnrealTeam.SB.Views.Interfaces
{
    public interface IEntity
    {
        public void ConvertToEntity(EcsWorld ecsWorld, int entity);
    }
}