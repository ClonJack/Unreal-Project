using Leopotam.EcsLite;

namespace UnrealTeam.SB.GamePlay.Views.Interfaces
{
    public interface IEntity
    {
        public void ConvertToEntity(EcsWorld ecsWorld, int entity);
    }
}